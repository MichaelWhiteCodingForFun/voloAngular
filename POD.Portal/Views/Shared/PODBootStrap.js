//
//  angular bootup and routing table
//


console.log("POD Bootstrap");

(function () {

    var app = angular.module('podApp', ['ngMaterial', 'ngRoute', 'pascalprecht.translate', 'ui.bootstrap', 'ngSanitize', 'blockUI', 'ngMessages']);

    app.config(['$controllerProvider', '$provide', '$httpProvider', function ($controllerProvider, $provide, $httpProvider) {

        app.register =
          {
              controller: $controllerProvider.register,
              service: $provide.service
          };

        

    }]);

    app.config(['$translateProvider', function ($translateProvider) {
        $translateProvider.useUrlLoader('/api/translation');
        $translateProvider.useSanitizeValueStrategy(null);
    }]);


    angular.module('podApp').config(function ($httpProvider) {
        $httpProvider.interceptors.push('apiInterceptorService');
    });


})();

console.log("POD Bootstrap FINISHED 2");




