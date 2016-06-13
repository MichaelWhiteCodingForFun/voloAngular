(function () {
    'use strict';
    angular.module('podApp').register.controller('setPasswordController', ['$location', '$scope', 'authService', 'ajaxService', '$routeParams', function ($location,$scope, authService, ajaxService, $routeParams) {

        var vm = this;

        this.initializeController = function () {

            $scope.id = $routeParams.userId;
            $scope.code = $routeParams.code;

            $scope.message = "";
            $scope.minlength = "6";
            $scope.maxlength = "20";

            $scope.showForm = false;
            $scope.errMessage = "";
            $scope.scsMessage = "";
            $scope.IsResetMode = false;

            $scope.password = $scope.password;
            $scope.confirmpassword = $scope.confirmpassword;
            $scope.userName = "";
            if (authService.getAuthData() != null) {
                authService.logOut().then(function () {
                    $scope.checkPermission();
                }).catch(function () {
                    $scope.checkPermission();
                });
            }
            

            ajaxService.ajaxGet("api/users/GetUserNameById/" + $scope.id, function (response) {
                $scope.userName = response.userName;
                $scope.IsResetMode = response.statusID == 2;                
                $scope.showForm = true;
                $scope.ROP = $scope.IsResetMode == true ? "Reset Password" : "Create Password";
                $scope.resetTitle = $scope.IsResetMode == true ? "Reset Password" : "Create Password";
                $scope.resetMessage = $scope.IsResetMode == true ? "Please reset your password to access your account" : "Please create a password to activate your account";
            }, function () {

            });
        }

        this.resetPassword = function () {           
            authService.setPassword($scope.userName, $scope.code, $scope.password, $scope.confirmpassword).then(function (response) {               
                $scope.showForm = false;
                $scope.errMessage = "";
                $scope.scsMessage = response;
            }).catch(function (err) {
                $scope.errMessage = err.message;
                $scope.scsMessage = "";
            });
        };






    }]);
})();