(function() {
    'use strict';

    angular.module('podApp').factory('apiInterceptorService', APIInterceptorService);

    /* @ngInject */
    function APIInterceptorService($q, $injector, $location) {

        var apiInterceptorServiceFactory = {};

       var _responseError = function(rejection) {

            var authService = $injector.get('authService');

            if (rejection.status === 401) {
                authService.logOut();
                window.location = '/account/login?referer=' + $location.absUrl();
            }

            return $q.reject(rejection);
        }

        apiInterceptorServiceFactory.responseError = _responseError;

        return apiInterceptorServiceFactory;

    }

})();