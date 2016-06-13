
console.log("Ticket controller load");

angular.module("podApp").register.controller('TicketsController', ['$scope', '$routeParams', '$location', '$window', '$mdDialog', 'authService', 'ajaxService', 'dataGridService', 'alertService',
    function ($scope, $routeParams, $location, $window, $mdDialog, authService, ajaxService, dataGridService, alertService) {

        "use strict";

        var vm = this;

        this.initializeController = function () {

            vm.title = "Tickets";
            vm.errMessage = "";
            vm.scsMessage = "";
            vm.tickets = [];
            vm.organizations = [];
            vm.showDiv = false;
            vm.ticketOrganizationID = $window.localStorage['ticketOrganizationID'] ? $window.localStorage['ticketOrganizationID'] : null;

            dataGridService.initializeTableHeaders();
            dataGridService.addHeader("Account No", "accountNumber");
            dataGridService.addHeader("Account Name", "accountName");
            dataGridService.addHeader("Ticket Number", "ticketNumber");
            dataGridService.addHeader("Delivery Date", "deliveryDate");
            dataGridService.addHeader("Invoice Number", "invoiceNumber");
            dataGridService.addHeader("", "btnCol");
            vm.tableHeaders = dataGridService.setTableHeaders();
            vm.defaultSort = dataGridService.setDefaultSort("accountNo");

            vm.currentPageNumber = 1;
            vm.sortExpression = "accountNo";
            vm.sortDirection = "ASC";
            vm.pageSize = '10';
            vm.prevPageNumber = vm.currentPageNumber;
            vm.totalTickets = 0;
            vm.totalPages = 1;
            vm.accountNumber = "";
            vm.accountName = "";
            vm.ticketNumber = "";
            vm.deliveryDate = "";
            vm.invoiceNumber = "";


            ajaxService.ajaxPost("", "api/customers/GetOrganizationCustomers",
                function (response) {
                    vm.statusOptions = response;
                },
                function () {
                    console.log("Error getting roles");
                }
            );

//            this.getOrganizationsByParentID();

            this.getTickets();

        }

        this.showGrid = function() {
            vm.showDiv = (vm.ticketOrganizationID && vm.tickets.length && (authService.getAuthData().user.role == "Company Admin" || authService.getAuthData().user.role == "Company User")) ? true : false;
        }

        this.changeSorting = function (column) {
            dataGridService.changeSorting(column, vm.defaultSort, vm.tableHeaders);
            vm.defaultSort = dataGridService.getSort();
            vm.sortDirection = dataGridService.getSortDirection();
            vm.sortExpression = dataGridService.getSortExpression();
            vm.currentPageNumber = 1;
            vm.getTickets();
        };

        this.DocGet = function (ticket) {
            ajaxService.ajaxPost(ticket, "api/tickets/GetDocumentContent",
               function (response) {
                   var file = new Blob([response], { type: 'application/pdf' });
                   var fileURL = URL.createObjectURL(file);
                   vm.content = fileURL;
                   vm.scsMessage = fileURL;
               },
               function (response) {
                   vm.errMessage = ajaxService.hanldeErrorMessage(response);
                   vm.scsMessage = "";
               }
           );
        }
        this.setSortIndicator = function (column) {
            return dataGridService.setSortIndicator(column, vm.defaultSort);
        };

        this.pageChanged = function () {
            vm.getTickets();
        }

        this.getTickets = function () {
            var ticketInquiry = vm.createTicketInquiryObject();
            ajaxService.ajaxPost(ticketInquiry, "api/tickets/gettickets", this.getTicketsOnSuccess, this.getTicketsOnError);
        }

        this.getOrganizationsByParentIDOnSuccess = function (response) {
            vm.organizations = Array.isArray(response.organizations) ? response.organizations : [];
        }

        this.getOrganizationsByParentIDOnError = function (response) {
            alertService.renderErrorMessage(ajaxService.hanldeErrorMessage(response));
        }

        this.getTicketOrganizationData = function (data) {
            if (data) {
                $window.localStorage.setItem('ticketOrganizationID', JSON.stringify(data.id));
                vm.ticketOrganizationID = data.id;
            } else {
                $window.localStorage.removeItem('ticketOrganizationID');
                vm.ticketOrganizationID = null;
            }
            
            vm.getTickets();
        }

        this.createTicketInquiryObject = function () {
            var ticketInquiry = new Object();
            ticketInquiry.organizationID = vm.ticketOrganizationID;
            ticketInquiry.accountNumber = vm.accountNumber;
            ticketInquiry.accountName = vm.accountName;
            ticketInquiry.ticketNumber = vm.ticketNumber;
            ticketInquiry.deliveryDate = vm.deliveryDate;
            ticketInquiry.invoiceNumber = vm.invoiceNumber;
            ticketInquiry.currentPageNumber = vm.currentPageNumber;
            ticketInquiry.sortExpression = vm.sortExpression;
            ticketInquiry.sortDirection = vm.sortDirection;
            ticketInquiry.pageSize = vm.pageSize;

            return ticketInquiry;

        }

        this.getTicketsOnSuccess = function (response) {
            vm.tickets = Array.isArray(response.tickets) ? response.tickets : [];
            vm.totalTickets = response.totalRows;
            vm.totalPages = Math.ceil(vm.totalTickets / vm.pageSize);
            vm.showGrid();
        }

        this.getTicketsOnError = function (response) {
            alertService.renderErrorMessage(ajaxService.hanldeErrorMessage(response));
        }

        this.loadData = function () {
            vm.getReports();
        }
    }]);
