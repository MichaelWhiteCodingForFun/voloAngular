
console.log("customer inquiry controller load");

angular.module("podApp").register.controller('CustomerMaintenanceController', ['$scope', '$routeParams', '$location', '$window', '$mdDialog', 'ajaxService', 'dataGridService', 'alertService',
    function ($scope, $routeParams, $location, $window, $mdDialog, ajaxService, dataGridService, alertService) {

        "use strict";

        var vm = this;

        this.initializeController = function () {

            vm.title = "Customer Maintenance";
            vm.errMessage = "";
            vm.scsMessage = "";
            vm.customers = [];
            vm.roleOptions = [];
            vm.alerts = [];
            vm.closeAlert = alertService.closeAlert;
            vm.originalData = [];
            vm.needAddCustomer = false;
            dataGridService.initializeTableHeaders();

            dataGridService.addHeader("Account Number", "AccountNumber");
            dataGridService.addHeader("Account Name", "OrganizationName");
            dataGridService.addHeader("Admin User", "FullName");
            dataGridService.addHeader("Email Address", "Email");
            dataGridService.addHeader("Status", "StatusDisplayName");
            dataGridService.addHeader("Last Login", "LastLogin");
            dataGridService.addHeader("", "Edit");

            vm.tableHeaders = dataGridService.setTableHeaders();
            vm.defaultSort = dataGridService.setDefaultSort("Account Name");


            vm.accountNumber = "";
            vm.accountName = "";
            vm.lastLoginDate = "";
            vm.adminUser = "";
            vm.userName = "";
            vm.statusName = "";
            vm.currentPageNumber = 1;
            vm.sortExpression = "OrganizationName";
            vm.sortDirection = "ASC";
            vm.pageSize = '10';
            vm.addedCustomers = [];
            vm.prevPageNumber = vm.currentPageNumber;
            vm.totalCustomers = 0;
            vm.totalPages = 1;
            vm.defaultOrganizationName = '';

            this.getCustomers();
//            $scope.$watch('vm.currentPageNumber', function (newValue, oldValue) {
//                vm.prevPageNumber = oldValue;
//            });

        }

        this.closeAlert = function (index) {
            vm.alerts.splice(index, 1);
        };

        this.changeSorting = function (column) {
            if (vm.alertSaveCustomer()) return;
            dataGridService.changeSorting(column, vm.defaultSort, vm.tableHeaders);

            vm.defaultSort = dataGridService.getSort();
            vm.sortDirection = dataGridService.getSortDirection();
            vm.sortExpression = dataGridService.getSortExpression();
            vm.currentPageNumber = 1;

            vm.getCustomers();
           
        };

        this.setSortIndicator = function (column) {
            return dataGridService.setSortIndicator(column, vm.defaultSort);
        };

        this.pageChanged = function () {
            if (vm.alertSaveCustomer()) return;
            vm.getCustomers();
        }

        this.getCustomers = function () {
            var customerInquiry = vm.createCustomerInquiryObject();
            ajaxService.ajaxPost(customerInquiry, "api/customers/GetCustomers", this.getCustomersOnSuccess, this.getCustomersOnError);
        }

        this.createCustomerInquiryObject = function () {

            var customerInquiry = new Object();

            customerInquiry.accountNumber = vm.accountNumber,
            customerInquiry.accountName = vm.accountName,
            customerInquiry.statusName = vm.statusName,
            customerInquiry.userName = vm.userName,
            customerInquiry.adminUser = vm.adminUser,
            customerInquiry.lastLoginDate = vm.lastLoginDate,

            customerInquiry.currentPageNumber = vm.currentPageNumber;
            customerInquiry.sortExpression = vm.sortExpression;
            customerInquiry.sortDirection = vm.sortDirection;
            customerInquiry.pageSize = vm.pageSize;

            return customerInquiry;

        }

        this.getCustomersOnSuccess = function (response) {
            vm.customers = Array.isArray(response.customers) ? response.customers : [];
            vm.totalCustomers = response.totalRows + vm.addedCustomers.length;
            for (var i = 0; i < vm.addedCustomers.length; i++) {
                vm.addedCustomers[i].selected = true;
                vm.customers.push(vm.addedCustomers[i]);
            }
            vm.addedCustomers = [];
            vm.totalPages = Math.ceil(vm.totalCustomers / vm.pageSize);
        }

        this.getCustomersOnError = function (response) {
            alertService.renderErrorMessage(ajaxService.hanldeErrorMessage(response));
        }

        this.getTemplate = function (customer) {
            if (customer.selected) {
                return 'edit';
            }
            else return 'display';
        };

        this.editCustomer = function (customer) {
            if (vm.alertSaveCustomer()) return;
            angular.forEach(vm.customers, function (value) {
                value.selected = false;
            });
            vm.originalData = angular.copy(customer);
            customer.selected = true;
        };

        this.reset = function (customer) {
            if (customer.id) {
                angular.copy(vm.originalData, customer);
                customer.selected = false;
            } else {
                vm.customers.splice(vm.customers.indexOf(customer), 1);
                vm.addedCustomers.splice(vm.addedCustomers.indexOf(customer), 1);
                var currentPageNumber = vm.currentPageNumber;
                vm.totalCustomers--;
                vm.totalPages = vm.currentPageNumber = Math.ceil(vm.totalCustomers / vm.pageSize);
                if (vm.currentPageNumber < currentPageNumber) {
                    vm.pageChanged();
                }
            }
        };

        this.addCustomer = function () {
            if (vm.alertSaveCustomer(true)) return;
            var nextTotalPages = Math.ceil((vm.totalCustomers + 1) / vm.pageSize);
            if (nextTotalPages > vm.totalPages) {
                if (vm.alertSaveCustomer()) {
                    vm.needAddCustomer = true;
                    return;
                }
            }
            vm.totalCustomers++;
            vm.totalPages = nextTotalPages;
            if (vm.customers.length && vm.customers[0].role) {
                vm.defaultOrganizationName = vm.customers[0].role.organizationName;
            }
            var customer = {
                userName: vm.userName,
                fullName: vm.fullName,
                role: {
                    name: '',
                    id: '',
                    roleID: '',
                    organizationName: vm.defaultOrganizationName
                },
                selected: true
            };
            vm.addedCustomers.push(customer);
            var currentPageNumber = vm.currentPageNumber;
            vm.currentPageNumber = vm.totalPages;
            if (vm.currentPageNumber > currentPageNumber) {
                vm.pageChanged();
            } else {
                vm.customers.push(customer);
            }
//            vm.editCustomer(vm.customers[vm.customers.length - 1]);
        };

        this.addCustomertoDB = function (customer) {
            ajaxService.ajaxPost(customer, "api/customers/add", function (response) {
                vm.getCustomers();
                vm.errMessage = "";
                vm.scsMessage = response;
                vm.addedCustomers.splice(vm.addedCustomers.indexOf(customer), 1);
                vm.addNewPageCustomer();
            }, function (response) {
                vm.errMessage = ajaxService.hanldeErrorMessage(response);
                vm.scsMessage = "";
            });

        };

        this.addNewPageCustomer = function () {
            if (vm.needAddCustomer) {
                if (vm.customers.length && vm.customers[0].role) {
                    vm.defaultOrganizationName = vm.customers[0].role.organizationName;
                }
                var newCustomer = {
                    userName: vm.userName,
                    fullName: vm.fullName,
                    role: {
                        name: '',
                        id: '',
                        roleID: '',
                        organizationName: vm.defaultOrganizationName
                    },
                    selected: true
                };
                vm.addedCustomers.push(newCustomer);
                var currentPageNumber = vm.currentPageNumber;
                vm.totalCustomers++;
                vm.totalPages = vm.currentPageNumber = Math.ceil(vm.totalCustomers / vm.pageSize);
                vm.needAddCustomer = false;
                if (vm.currentPageNumber > currentPageNumber) {
                    vm.pageChanged();
                }
            }
        };

        this.updateCustomer = function (customer) {
            ajaxService.ajaxPost(customer, "api/customers/updateCustomer",
                function (response) {
                    vm.reset(customer);
                    vm.getCustomers();
                    vm.errMessage = "";
                    vm.scsMessage = response;
                },
                function (response) {
                    vm.errMessage = ajaxService.hanldeErrorMessage(response);
                    vm.scsMessage = "";
                }
            );

        };

        this.deleteCustomer = function (customer) {
            ajaxService.ajaxPost(customer, "api/customers/deleteCustomer",
                function () {
                    vm.getCustomers();
                },
                function () {
                    console.log("Error deleting");
                }
            );
        };

        this.manageCustomer = function(customer) {
            $window.localStorage.setItem('organizationData', customer.id);
            $location.path("User/UserMaintenance");
        }

        this.addOrUpdateCustomer = function (customer) {
            if (customer.id) {
                vm.updateCustomer(customer);
            } else {
                vm.addCustomertoDB(customer);
            }
        }

        this.alertSaveCustomer = function (checkSaved) {
            if (vm.customers.filter(function (customer) {
                return customer.selected && ((checkSaved && customer.id) || !checkSaved);
            }).length) {
                vm.showDialog("Please save your changes");
                return true;
            }
            return false;
        };

        this.showDialog = function (message) {

            var alert = $mdDialog.alert({
                controller: ['$scope', function ($scope) {
                    $scope.message = message;
                    $scope.loadData = vm.loadData.bind();
                    $scope.closeDialog = vm.closeDialog.bind();
                }],
                templateUrl: '/Views/Dialog/ConfirmSaveDialog.html',
            });

            $mdDialog.show(alert).finally(function () {
                alert = undefined;
            });
        }


        this.closeDialog = function () {
            $mdDialog.hide();
        };

        this.cancelButton = function () {
            vm.closeDialog();
//            vm.currentPageNumber = vm.prevPageNumber;
        };

        this.loadData = function () {
            vm.closeDialog();
            vm.needAddCustomer = false;
            vm.addedCustomers = [];
            vm.getCustomers();
        }

    }]);
