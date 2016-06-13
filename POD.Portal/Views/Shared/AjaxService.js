
console.log("ajax service");

angular.module('podApp').service('ajaxService', ['$http', 'blockUI', function ($http, blockUI) {

    "use strict";

    this.ajaxPost = function(data, route, successFunction, errorFunction) {

        blockUI.start();

        $http.post(route, data).success(function(response, status, headers, config) {
            blockUI.stop();
            successFunction(response, status);
        }).error(function(response) {
            blockUI.stop();
            errorFunction(response);
        });

    };
    this.ajaxGet = function(route, successFunction, errorFunction) {

        blockUI.start();

        $http.get(route).success(function(response, status, headers, config) {
            blockUI.stop();
            successFunction(response, status);
        }).error(function(response) {
            blockUI.stop();
            errorFunction(response);
        });

    };
    this.hanldeErrorMessage = function(error) {
        if (error && error.modelState && error.modelState.error && error.modelState.error[0]) {
            return error.modelState.error[0];
        } else if (error.exceptionMessage) {
            return error.exceptionMessage;
        } else if (error.message) {
            return error.message;
        } else {
            return "An error occured";
        }
        
    };

}]);