(function () {
    'use strict';
    
    angular.module('podApp').factory('authService', ['$http', '$q', '$timeout', '$window', 'ajaxService',
        function ($http, $q, $timeout, $window, ajaxService) {
            var authServiceFactory = {};

            var _authentication = {
                isAuth: false,
                user: false
            };

            var _setPassword = function (userName, code, password, confirmpassword) {

                _deleteAuthData();

                var deferred = $q.defer();

                ajaxService.ajaxPost({ Email: userName, Code: code, Password: password, ConfirmPassword: confirmpassword }, 'api/Accounts/ResetPassword',
                    function (response) {
                        deferred.resolve(response);
                    },
                    function (err) {
                        var resMessage = ajaxService.hanldeErrorMessage(err);

                        deferred.reject({ message: resMessage });
                    });

                    return deferred.promise;

            };

            var _deleteAuthData = function () {
                $window.localStorage.removeItem('authorizationData');
                $window.localStorage.removeItem('organizationData');
                $window.sessionStorage.removeItem('authorizationData');
            };

            var _getUserIdentity = function (callback) {

                ajaxService.ajaxGet('api/users/GetUserIdentity',
                    function (response) {
                        console.log(response);
                        _authentication.user = response;
                        $window.localStorage.setItem('authorizationData', JSON.stringify({ user: _authentication.user }));
                        callback && callback();
                    },
                    function (err) {
                        console.log(err);
                    }
                );
            }

            var _login = function (loginData) {

                _deleteAuthData();

                var deferred = $q.defer();

                ajaxService.ajaxPost({ UserName: loginData.UserName, Password: loginData.Password, RememberMe: loginData.RememberMe }, 'api/Accounts/Login',
                    function (response) {

                        $window.localStorage.setItem('lastLoginUserName', loginData.UserName);

                        _authentication.isAuth = true;

                        authServiceFactory.getUserIdentity(function() {
                            deferred.resolve(response);
                        });
                    },
                    function (err) {
                        _deleteAuthData();

                        var resMessage = ajaxService.hanldeErrorMessage(err);

                        deferred.reject({ message: resMessage });
                    }
                );

                return deferred.promise;
            };

            var _getAuthData = function () {
                var authData = $window.sessionStorage['authorizationData'] ? $window.sessionStorage['authorizationData'] : $window.localStorage['authorizationData'];
                return authData ? JSON.parse(authData) : null;

            };

            var _logOut = function () {
                var deferred = $q.defer();

                ajaxService.ajaxGet('api/accounts/LogOut',
                function () {
                    _deleteAuthData();

                    _authentication.isAuth = false;
                    _authentication.user = false;
                    deferred.resolve();

                },
                function () {
                    _deleteAuthData();

                    _authentication.isAuth = false;
                    _authentication.user = false;

                    deferred.reject();
                });

                return deferred.promise;

            };

            var _forgotPassword = function (resetData) {

                _deleteAuthData();

                var deferred = $q.defer();

                ajaxService.ajaxPost({ Email: resetData }, 'api/accounts/Forgotpassword',
                    function (response) {
                        deferred.resolve(response);
                    },
                    function (response) {
                        deferred.reject({ message: ajaxService.hanldeErrorMessage(response) });
                    });

                return deferred.promise;

            };

            authServiceFactory.getUserIdentity = _getUserIdentity;
            authServiceFactory.setPassword = _setPassword;
            authServiceFactory.login = _login;
            authServiceFactory.forgotPassword = _forgotPassword;
            authServiceFactory.deleteAuthData = _deleteAuthData;
            authServiceFactory.logOut = _logOut;
            authServiceFactory.getAuthData = _getAuthData;
            return authServiceFactory;

        }]);
})();