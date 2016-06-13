
console.log("report inquiry controller load");

angular.module("podApp").register.controller('ReportController', ['$scope', '$routeParams', '$location', '$window', '$mdDialog', 'ajaxService', 'dataGridService', 'alertService',
    function ($scope, $routeParams, $location, $window, $mdDialog, ajaxService, dataGridService, alertService) {

        "use strict";

        var vm = this;

        this.initializeController = function () {

            vm.title = "Reports";
            vm.errMessage = "";
            vm.scsMessage = "";
            vm.reports = [];          
            vm.alerts = [];
            vm.closeAlert = alertService.closeAlert;           
            vm.organizationID = $window.localStorage['organizationData'] ? $window.localStorage['organizationData'] : "00000000-0000-0000-0000-000000000000";            
            dataGridService.initializeTableHeaders();

            dataGridService.addHeader("createdByID", "createdByID");
            dataGridService.addHeader("createdDateUtc", "createdDateUtc");
            dataGridService.addHeader("details", "details");
            dataGridService.addHeader("id", "_id");
            dataGridService.addHeader("isSuccess", "isSuccess");
            dataGridService.addHeader("reportTypeID", "reportTypeID");
            dataGridService.addHeader("ticketID", "ticketID");

            vm.tableHeaders = dataGridService.setTableHeaders();
            vm.defaultSort = dataGridService.setDefaultSort("Description");

            vm.currentPageNumber = 1;
            vm.sortExpression = "";
            vm.sortDirection = "ASC";
            vm.pageSize = '10';
            vm.addedReports = [];
            vm.prevPageNumber = vm.currentPageNumber;
            vm.totalPages = 1;
            vm.totalReports = 0;
            vm.defaultOrganizationName = '';

            vm.getReports();
        }

        this.closeAlert = function (index) {
            vm.alerts.splice(index, 1);
        };

        this.changeSorting = function (column) {
          
            dataGridService.changeSorting(column, vm.defaultSort, vm.tableHeaders);

            vm.defaultSort = dataGridService.getSort();
            vm.sortDirection = dataGridService.getSortDirection();
            vm.sortExpression = dataGridService.getSortExpression();
            vm.currentPageNumber = 1;

            vm.getReports();

        };

        this.setSortIndicator = function (column) {
            return dataGridService.setSortIndicator(column, vm.defaultSort);
        };

        this.pageChanged = function () {
            if (vm.alertSaveReport()) return;
            vm.getReports();
        }

        this.getReports = function () {
            var reportInquiry = vm.createReportInquiryObject();
            ajaxService.ajaxPost(reportInquiry, "api/report/reports", this.getReportsOnSuccess, this.getReportsOnError);
        }

        this.createReportInquiryObject = function () {

            var reportInquiry = new Object();

            reportInquiry.ReportTypeID = vm.ReportTypeID;
            reportInquiry.ReportTypeID = vm.ReportTypeID;
            reportInquiry.SrchString = vm.SrchString;
            reportInquiry.sortExpression = vm.sortExpression;
            reportInquiry.sortDirection = vm.sortDirection;
            reportInquiry.pageSize = vm.pageSize;

            return reportInquiry;

        }      

        this.getReportsOnSuccess = function (response) {           
           
            vm.reports = Array.isArray(response.reports) ? response.reports : [];           
            vm.totalReports = response.totalRows + vm.addedReports.length;
            vm.totalPages = Math.ceil(vm.totalReports / vm.pageSize);


            console.log("AAAAAAAAAAAAAAAAAAAAAAAA");
            console.log(vm.pageSize);
            console.log(vm.reports);
        }

        this.getReportsOnError = function (response) {
            alertService.renderErrorMessage(ajaxService.hanldeErrorMessage(response));
        }

        this.getTemplate = function (report) {
          return 'display';
        };

        this.deleteReport = function (report) {
            ajaxService.ajaxPost(report, "api/report/deleteReport",
                function (response) {
                    vm.getReports();
                    vm.errMessage = "";
                    vm.scsMessage = response;
                },
                function () {
                    vm.errMessage = ajaxService.hanldeErrorMessage(response);
                    vm.scsMessage = "";
                }
            );
        };       

        this.loadData = function () {
            vm.closeDialog();
            vm.needAddReport = false;
            vm.addedReports = [];
            vm.getReports();
        }


    }]);
