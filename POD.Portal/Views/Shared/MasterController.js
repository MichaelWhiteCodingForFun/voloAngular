
console.log("master controller");

angular.module('podApp').controller('masterController', ['$scope' , '$translate', '$routeParams', '$location', 'ajaxService', 'authService', 'applicationConfiguration',

    function ($scope, $translate, $routeParams, $location, ajaxService, authService, applicationConfiguration) {

        var vm = this;

        this.initializeController = function () {
            vm.isAuth = false;
            vm.isUser = false;
            vm.isCustomerAdmin = false;
            vm.isCompanyAdmin = false;
            vm.isOwner = false;            

            vm.applicationVersion = applicationConfiguration.version;

            $scope.checkPermission();

            var lang = 'en';
            setLanguage(lang);
        }

        $scope.setLanguage = setLanguage;

        function setLanguage(lang) {
            $translate.use(lang);
        }

        $scope.checkPermission = function() {

            vm.authData = authService.getAuthData();

            if (!vm.authData) {
                vm.isUser = false;
                vm.isCustomerAdmin = false;
                vm.isCompanyAdmin = false;
                vm.isOwner = false;
                vm.isAuth = false;
                return false;
            }

            if (vm.authData.user.role == "Customer Admin") {
                vm.isCustomerAdmin = true;
            } else if (vm.authData.user.role == "Company Admin") {
                vm.isCompanyAdmin = true;
            } else if (vm.authData.user.role == "System Admin") {
                vm.isOwner = true;
            } else if (vm.authData.user.role == "System User" || vm.authData.user.role == "Company User" || vm.authData.user.role == "Customer User") {
                vm.isUser = true;
            }
            vm.isAuth = true;
            return true;
        };

        this.logOut = function () {
            authService.logOut().then(function () {
                window.location.href = '/account/login';
            }).catch(function () {
                window.location.href = '/account/login';
            });
        };

    }
]);