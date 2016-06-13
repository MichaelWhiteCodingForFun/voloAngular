console.log("Login controller load");

angular.module("podApp").register.controller('LoginController', [
    '$scope', '$routeParams', '$location', '$window', '$translate', 'authService', 'ajaxService', 'dataGridService', 'alertService',
    function ($scope, $routeParams, $location, $window, $translate, authService, ajaxService, dataGridService, alertService) {

        "use strict";

        $scope.RememberMe = false;
        $scope.loaded = false;
        $scope.initalizeController = function () {
            var authData = authService.getAuthData();
            if (authData && authData.user && authData.user.id) {
                $scope.redirectByRole();
            } else {
                $scope.loaded = true;
            }
            
        };

        $scope.message = "";
        $scope.minlength = "6";
        $scope.maxlength = "20";

        $scope.loginData = {
            UserName: "",
            Password: "",
            RememberMe: false
        };

        $scope.login = function() {
            authService.login($scope.loginData).then(function (response) {
                $scope.checkPermission();
                if ($routeParams.referer) {
                    $location.path($routeParams.referer);
                } else {
                    $scope.redirectByRole();
                }
            }).catch(function (err) {
                $scope.message = err.message;
            });
        };
        $scope.redirectByRole = function () {
            if (authService.getAuthData().user.role == "System Admin" || authService.getAuthData().user.role == "Company Admin" || authService.getAuthData().user.role == "Customer Admin") {
                $location.path("/User/UserMaintenance");
            } else {
                $location.path("/Search/Tickets");
            }
        }
    }

]);
