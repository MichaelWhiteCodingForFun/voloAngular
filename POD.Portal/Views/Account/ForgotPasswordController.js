console.log("Forgotten Password controller load");

angular.module("podApp").register.controller('ForgetPasswordController', [
    '$scope', '$routeParams', '$location', 'authService', 'ajaxService', 'dataGridService', 'alertService',
    function($scope, $routeParams, $location, authService, ajaxService, dataGridService, alertService) {

        "use strict";

        $scope.UserName = {
            text: ''
        };

        $scope.showForm = true;
        $scope.errMessage = "";
        $scope.scsMessage = "";     
        $scope.forgotPassword = function (UserName) {
            authService.forgotPassword(UserName.text).then(function (response) {
                $scope.showForm = false;
                $scope.scsMessage = response;
                $scope.errMessage = "";
            }).catch(function (err) {
                $scope.scsMessage = "";
                $scope.errMessage = err.message;
            });
        };
    
        $scope.cancel = function () {
            $location.path("/Account/Login");
        }
    }
]);