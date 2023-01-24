(function () {

    Controller.$inject = ['$scope', 'assetsService', '$routeParams'];
    function Controller($scope, assetsService, $routeParams) {

        let $this = this;

        $this.getIsRegister = getIsRegister;

        let isRegister = false;

        init();

        function init() {

            isRegister = 'section' in $routeParams;

            assetsService.load(['https://cdn.passwordless.dev/dist/0.2.0/passwordless.iife.js'], $scope)
                .then(() => buttonState = 'init');
        }

        function getIsRegister() {
            return isRegister;
        }
    }

    angular.module('umbraco').controller('passwordless.main', Controller);

})()