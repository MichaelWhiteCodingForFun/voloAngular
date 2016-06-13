
console.log("alert PODRouting");

angular.module("podApp").config([
    '$routeProvider', '$locationProvider', '$translateProvider', 'applicationConfigurationProvider',
    function ($routeProvider, $locationProvider, $translateProvider,applicationConfigurationProvider) {
        var self = this;
        this.getApplicationVersion = function () {
            var applicationVersion = applicationConfigurationProvider.getVersion();
            return applicationVersion;
        },
        this.routes = {
            '/User/UserMaintenance': {
                roles: ["Company Admin", "Customer Admin", "System Admin"]
            },
            '/Customers/CustomerMaintenance': {
                roles: ["Company Admin", "System Admin"]
            },
            '/Report/Report': {
                roles: ["Company Admin", "Company User"]
            },
            '/Search/Tickets': {
                roles: ["Company Admin", "Company User", "Customer Admin", "Customer User"]
            }
        }
        this.checkSecurity = function ($timeout, $window, $q, route, authService) {
            var deferred = $q.defer();
            var authData = authService.getAuthData();
            if (authData && authData.user) {
                if (self.routes[route]) {
                    if (self.routes[route].roles.indexOf(authData.user.role) > -1) {
                        deferred.resolve();
                    } else {
                        deferred.reject();
                    }
                } else {
                    deferred.resolve();
                }
                
            } else {
                $timeout(function () {
                    deferred.resolve();
                }, 300);
            }
            return deferred.promise;
        };

        var baseSiteUrlPath = $("base").first().attr("href");

        $routeProvider.when('/:section',
        {
            templateUrl: function (rp) {
                return baseSiteUrlPath + 'views/' + rp.section + '/' + 'index.html?v=' + self.getApplicationVersion();
            },

            resolve: {
                load: [
                    '$q', '$rootScope', '$location', function ($q, $rootScope, $location) {

                        var path = $location.path().split("/");
                        var directory = path[1];
                        var controllerName = "index";

                        var controllerToLoad = "Views/" + directory + "/" + controllerName + "Controller.js?v=" + self.getApplicationVersion();

                        var deferred = $q.defer();
                        require([controllerToLoad], function (err) {
                            $rootScope.$apply(function () {
                                deferred.resolve();
                            });
                        }, function (err) {
                            if (err) {
                                deferred.reject();
                                $location.path("home/notfound");
                            }
                        });

                        return deferred.promise;

                    }
                ],
                loggedInAndHasPermission: [
                    '$q', '$location', '$timeout', '$window', 'authService', function ($q, $location, $timeout, $window, authService) {
                        return self.checkSecurity($timeout, $window, $q, $location.path(), authService).then(function () {
                            return;
                        }).catch(function() {
                            $location.path("home/notfound");
                        });
                    }
                ]
            }
        });

        $routeProvider.when('/:section/:tree',
        {
            templateUrl: function (rp) { return baseSiteUrlPath + 'views/' + rp.section + '/' + rp.tree + '.html?v=' + self.getApplicationVersion(); },

            resolve: {
                load: [
                    '$q', '$rootScope', '$location', function ($q, $rootScope, $location) {

                        var path = $location.path().split("/");
                        var directory = path[1];
                        var controllerName = path[2];

                        var controllerToLoad = "Views/" + directory + "/" + controllerName + "Controller.js?v=" + self.getApplicationVersion();

                        var deferred = $q.defer();
                        require([controllerToLoad], function (err) {
                            $rootScope.$apply(function () {
                                deferred.resolve();
                            });
                        }, function (err) {
                            if (err) {
                                deferred.reject();
                                $location.path("home/notfound");
                            }
                        });

                        return deferred.promise;

                    }
                ],
                loggedInAndHasPermission: [
                    '$q', '$location', '$timeout', '$window', 'authService', function ($q, $location, $timeout, $window, authService) {
                        return self.checkSecurity($timeout, $window, $q, $location.path(), authService).then(function () {
                            return;
                        }).catch(function () {
                            $location.path("home/notfound");
                        });
                    }
                ]
            }


        });

        $routeProvider.when('/:section/:tree/:id',
        {
            templateUrl: function (rp) { return baseSiteUrlPath + 'views/' + rp.section + '/' + rp.tree + '.html?v=' + self.getApplicationVersion(); },

            resolve: {
                load: [
                    '$q', '$rootScope', '$location', function ($q, $rootScope, $location) {

                        var path = $location.path().split("/");
                        var directory = path[1];
                        var controllerName = path[2];

                        var controllerToLoad = "Views/" + directory + "/" + controllerName + "Controller.js?v=" + self.getApplicationVersion();

                        var deferred = $q.defer();
                        require([controllerToLoad], function () {
                            $rootScope.$apply(function () {
                                deferred.resolve();
                            });
                        }, function (err) {
                            if (err) {
                                deferred.reject();
                                $location.path("home/notfound");
                            }
                        });

                        return deferred.promise;

                    }
                ],
                loggedInAndHasPermission: [
                    '$q', '$location', '$timeout', '$window', 'authService', function ($q, $location, $timeout, $window, authService) {
                        return self.checkSecurity($timeout, $window, $q, $location.path(), authService).then(function () {
                            return;
                        }).catch(function () {
                            $location.path("home/notfound");
                        });
                    }
                ]
            }

        });

        $routeProvider.when('/',
        {
            templateUrl: function (rp) { return baseSiteUrlPath + 'views/account/login.html?v=' + self.getApplicationVersion(); },

            resolve: {
                load: [
                    '$q', '$rootScope', '$location', function ($q, $rootScope, $location) {

                        var controllerToLoad = "Views/Account/LoginController.js?v=" + self.getApplicationVersion();

                        var deferred = $q.defer();
                        require([controllerToLoad], function () {
                            $rootScope.$apply(function () {
                                deferred.resolve();
                            });
                        }, function (err) {
                            if (err) {
                                deferred.reject();
                                $location.path("home/notfound");
                            }
                        });

                        return deferred.promise;

                    }
                ],
                loggedInAndHasPermission: [
                    '$q', '$location', '$timeout', '$window', 'authService', function ($q, $location, $timeout, $window, authService) {
                        return self.checkSecurity($timeout, $window, $q, $location.path(), authService).then(function () {
                            return;
                        }).catch(function () {
                            $location.path("home/notfound");
                        });
                    }
                ]
            }


        });

        $routeProvider.otherwise({
            redirectTo: "home/notfound"
        });

        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });

    }
]);