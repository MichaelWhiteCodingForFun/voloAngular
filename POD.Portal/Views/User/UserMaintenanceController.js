
console.log("user inquiry controller load");

angular.module("podApp").register.controller('UserMaintenanceController', ['$q', '$scope', '$routeParams', '$location', '$window', '$mdDialog', 'ajaxService', 'dataGridService', 'alertService',
    function ($q, $scope, $routeParams, $location, $window, $mdDialog, ajaxService, dataGridService, alertService) {

        "use strict";

        var vm = this;

        this.initializeController = function () {

            vm.title = "User Maintenance";
            vm.errMessage = "";
            vm.scsMessage = "";
            vm.users = [];
            vm.roleOptions = [];
            vm.statusColumnEdit = "";
            vm.statusOptions = [];
            vm.alerts = [];
            vm.closeAlert = alertService.closeAlert;
            vm.originalData = [];
            vm.organizationID = $window.localStorage['organizationData'] ? $window.localStorage['organizationData'] : null;
            vm.needAddUser = false;
            dataGridService.initializeTableHeaders();

            dataGridService.addHeader("Organisation", "OrganizationName");
            dataGridService.addHeader("Name", "FullName");
            dataGridService.addHeader("Email Address", "User");
            dataGridService.addHeader("Access Level", "RoleName");
            dataGridService.addHeader("Status", "StatusDisplayName");
            dataGridService.addHeader("Last Login", "LastLogin");
            dataGridService.addHeader("", "Edit");

            vm.tableHeaders = dataGridService.setTableHeaders();
            vm.defaultSort = dataGridService.setDefaultSort("Account Name");

            vm.fullName = "";
            vm.userName = "";
            vm.roleName = "";
            vm.statusName = "";
            vm.lastLoginDate = "";
            vm.currentPageNumber = 1;
            vm.sortExpression = "OrganizationName";
            vm.sortDirection = "ASC";
            vm.pageSize = '10';
            vm.addedUsers = [];
            vm.prevPageNumber = vm.currentPageNumber;
            vm.totalPages = 1;
            vm.totalUsers = 0;
            vm.defaultOrganizationName = '';
           
            ajaxService.ajaxPost("", "api/users/Roles",
                function (response) {
                    vm.roleOptions = response;
                },
                function () {
                    console.log("Error getting roles");
                }
            );

            ajaxService.ajaxPost("", "api/users/AccountStatuses",
                function (response) {
                    vm.statusOptions = response;
                },
                function () {
                    console.log("Error getting roles");
                }
            );

            vm.getUsers();
        }

        
        this.closeAlert = function (index) {
            vm.alerts.splice(index, 1);
        };

        this.clearMessages = function () {
            vm.errMessage = "";
            vm.scsMessage = "";
        };

        this.changeSorting = function (column) {
            if (vm.alertSaveUser()) return;
            dataGridService.changeSorting(column, vm.defaultSort, vm.tableHeaders);

            vm.defaultSort = dataGridService.getSort();
            vm.sortDirection = dataGridService.getSortDirection();
            vm.sortExpression = dataGridService.getSortExpression();
            vm.currentPageNumber = 1;

            vm.getUsers();

        };

        this.setSortIndicator = function (column) {
            return dataGridService.setSortIndicator(column, vm.defaultSort);
        };

        this.pageChanged = function () {
            if(vm.alertSaveUser()) return;
            vm.getUsers();
        }

        this.getUsers = function (clearMessages) {
            if (clearMessages)
                vm.clearMessages();
            var userInquiry = vm.createUserInquiryObject();
            ajaxService.ajaxPost(userInquiry, "api/users/GetUsers", this.getUsersOnSuccess, this.getUsersOnError);
        }

        this.createUserInquiryObject = function () {

            var userInquiry = new Object();
            
            userInquiry.organizationID = vm.organizationID;
            userInquiry.organizationName = vm.organizationName;
            userInquiry.fullName = vm.fullName;
            userInquiry.userName = vm.userName;
            userInquiry.roleName = vm.roleName;
            userInquiry.statusName = vm.statusName;
            userInquiry.lastLoginDate = vm.lastLoginDate;

            userInquiry.currentPageNumber = vm.currentPageNumber;
            userInquiry.sortExpression = vm.sortExpression;
            userInquiry.sortDirection = vm.sortDirection;
            userInquiry.pageSize = vm.pageSize;

            return userInquiry;

        }

        this.getUsersOnSuccess = function(response) {
            //var addedUsers = [];

            vm.users = Array.isArray(response.users) ? response.users : [];
            
//            if (vm.roleOptions.length && !vm.roleOptions[0].organizationName && vm.users.length) {
//                for (var i = 0; i < vm.roleOptions.length; i++) {
//                    vm.roleOptions[i].organizationName = vm.users[0].organization.organizationName;
//                }
//            }
            vm.totalUsers = response.totalRows + vm.addedUsers.length;
            for (var i = 0; i < vm.addedUsers.length; i++) {
                vm.addedUsers[i].selected = true;
                vm.users.push(vm.addedUsers[i]);
            }
            
            //var lastPageUsers = Math.ceil(response.totalRows / vm.pageSize);
            //var sliceCurrPage = vm.currentPageNumber - lastPageUsers <= 0 ? 1 : (vm.currentPageNumber - (lastPageUsers - 1));
            //if (vm.addedUsers.length && vm.currentPageNumber - lastPageUsers >= 0) {
            //    var start = sliceCurrPage !== 1 ? response.totalRows % vm.pageSize : 0;
            //    var startSlice = (sliceCurrPage - 1) * vm.pageSize - start;
            //    var sliceCount = vm.users.length ? vm.pageSize - start : vm.pageSize;
            //    addedUsers = vm.addedUsers.slice(startSlice, (vm.addedUsers.length < sliceCount ? vm.addedUsers.length : startSlice + sliceCount + 1));
            //    for (var i = 0; i < addedUsers.length; i++) {
            //        addedUsers[i].selected = true;
            //        vm.users.push(addedUsers[i]);
            //    }
                
            //}
            
            vm.totalPages = Math.ceil(vm.totalUsers / vm.pageSize);
        }

        this.getUsersOnError = function (response) {
            alertService.renderErrorMessage(ajaxService.hanldeErrorMessage(response));
        }

        this.getTemplate = function (user) {
            if (user.selected) {
                return 'edit';
            }
            else return 'display';
        };

        this.editUser = function (user) {
            if (vm.alertSaveUser()) return;
            angular.forEach(vm.users, function (value) {
                value.statusID = value.statusID.toString();
                value.selected = false;
            });
            vm.originalData = angular.copy(user);
            user.selected = true;
            vm.statusChanged(user.statusID);
        };

        this.reset = function (user) {
            if (user.id) {
                angular.copy(vm.originalData, user);
                user.selected = false;
            } else {
                vm.users.splice(vm.users.indexOf(user), 1);
                vm.addedUsers.splice(vm.addedUsers.indexOf(user), 1);
                var currentPageNumber = vm.currentPageNumber;
                vm.totalUsers--;
                vm.totalPages = vm.currentPageNumber = Math.ceil(vm.totalUsers / vm.pageSize);
                if (vm.currentPageNumber < currentPageNumber) {
                    vm.pageChanged();
                }
            }
        };

        this.addUser = function () {
            if (vm.alertSaveUser(true)) return;
            var nextTotalPages = Math.ceil((vm.totalUsers + 1) / vm.pageSize);
            if (nextTotalPages > vm.totalPages) {
                if (vm.alertSaveUser()) {
                    vm.needAddUser = true;
                    return;
                }
            }
            vm.totalUsers++;
            vm.totalPages = nextTotalPages;
            if (vm.users.length && vm.users[0].organization) {
                vm.defaultOrganizationName = vm.users[0].organization.organizationName;
            }
            var user = {
                userName: vm.userName,
                fullName: vm.fullName,
                role: "",
                organization: vm.users[0].organization,
                statusID: 1, // Inactive
                organizationName: vm.defaultOrganizationName,
                selected: true
            };

            vm.addedUsers.push(user);
            var currentPageNumber = vm.currentPageNumber;
            vm.currentPageNumber = vm.totalPages;
            if (vm.currentPageNumber > currentPageNumber) {
                vm.pageChanged();
            } else {
                vm.users.push(user);
            }
        };

        this.showAll = function () {
            $window.localStorage.removeItem('organizationData');
            vm.organizationID = null;            
            vm.getUsers();
        }

        this.addUsertoDB = function (user) {
            ajaxService.ajaxPost(user, "api/accounts/register", function (response) {               
                vm.getUsers();
                vm.errMessage = "";
                vm.scsMessage = response[1];
                vm.addedUsers.splice(vm.addedUsers.indexOf(user), 1);
            }, function (response) {
                vm.errMessage = ajaxService.hanldeErrorMessage(response);
                vm.scsMessage = "";
            });

        };

        this.addNewPageUser = function() {
            if (vm.needAddUser) {
                if (vm.users.length && vm.users[0].organization) {
                    vm.defaultOrganizationName = vm.users[0].organization.organizationName;
                }
                var newUser = {
                    userName: vm.userName,
                    fullName: vm.fullName,
                    role: "",
                    organizationName: vm.defaultOrganizationName,
                    selected: true
                };
                vm.addedUsers.push(newUser);
                var currentPageNumber = vm.currentPageNumber;
                vm.totalUsers++;
                vm.totalPages = vm.currentPageNumber = Math.ceil(vm.totalUsers / vm.pageSize);
                vm.needAddUser = false;
                if (vm.currentPageNumber > currentPageNumber) {
                    vm.pageChanged();
                }
            }
        };

        this.updateUser = function (user) {
            ajaxService.ajaxPost(user, "api/users/updateUser",
                function (response) {                   
                    vm.reset(user);
                    vm.getUsers();
                    vm.errMessage = "";
                    vm.scsMessage = response;
                },
                function (response) {
                    vm.errMessage = ajaxService.hanldeErrorMessage(response);
                    vm.scsMessage = "";
                }
            );

        };
        

        this.deleteUser = function (user) {
            ajaxService.ajaxPost(user, "api/users/deleteUser",
                function (response) {                    
                    vm.getUsers();
                    vm.closeDialog();
                    vm.errMessage = "";
                    vm.scsMessage = response;
                },
                function (response) {
                    vm.errMessage = ajaxService.hanldeErrorMessage(response);
                    vm.scsMessage = "";
                }
            );
        };

        this.statusChanged = function (statusID) {
            if (statusID == 3) {
                vm.statusColumnEdit = false;
            } else {
                vm.statusColumnEdit = true;
            }
        }

        this.addOrUpdateUser = function (user) {            
            if (user.id) {
                vm.updateUser(user);
            } else {
                user.organization.organizationID = vm.organizationID;
                user.statusID = 1;
                vm.addUsertoDB(user);
            }
        }

        //this.addUsersRecusivley = function () {
        //   var i = 0;
        //   function next() {
        //        if (i < vm.addedUsers.length) {
        //            vm.registerUser(vm.addedUsers[i++]).then(next);
        //        } else {
        //            vm.addedUsers = [];
        //            vm.getUsers(false);
        //        }
        //    }
        //    next();
        //};

        this.addUsertoDBMultiple = function (user) {
            var defered = $q.defer();
            ajaxService.ajaxPost(user, "api/accounts/register", function (response) {
                user.id = response[0];
                //vm.originalData = angular.copy(vm.addedUsers[i]);
                //vm.reset(vm.addedUsers[i]);
                vm.scsMessage += response[1] + '<br/>';
                defered.resolve();
            }, function (response) {
                vm.errMessage += ajaxService.hanldeErrorMessage(response);
                defered.resolve();
            });
            return defered.promise;
        };

        this.addAllUsers = function () {           
            vm.clearMessages();          
            var i = 0;
            function next() {
                if (i < vm.addedUsers.length) {
                    vm.addUsertoDBMultiple(vm.addedUsers[i++]).then(next);
                } else {
                    vm.addedUsers = [];
                    vm.getUsers(false);
                }
            }
            next();            
        };

        this.addAllValid = function () {
            angular.forEach(vm.users, function (user) {
                if (!user.id) {
                    user.submitted = true;
        }
            });
            return true;
        }

        this.alertSaveUser = function (checkSaved) {
            if(vm.users.filter(function (user) {
                return user.selected && ((checkSaved && user.id) || !checkSaved);
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
                    $scope.addAllUsers = vm.addAllUsers.bind(vm);
                    $scope.loadData = vm.loadData.bind();
                    $scope.closeDialog = vm.closeDialog.bind();
            }],
                    templateUrl: '/Views/Dialog/ConfirmSaveDialog.html',
        });

        $mdDialog.show(alert).finally(function () {
            alert = undefined;
        });
        }

        this.showDeleteDialog = function (user) {
            var alert = $mdDialog.alert({
                    controller: ['$scope', function ($scope) {
                    $scope.deleteUser = vm.deleteUser.bind(vm, user);
                    $scope.closeDialog = vm.closeDialog.bind();
            }],
                    templateUrl: '/Views/Dialog/ConfirmDeleteDialog.html',
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
            vm.currentPageNumber = vm.prevPageNumber;
        };

        this.loadData = function () {
            vm.closeDialog();
            vm.needAddUser = false;
            vm.addedUsers =[];
            vm.getUsers();
        }


    }]);
