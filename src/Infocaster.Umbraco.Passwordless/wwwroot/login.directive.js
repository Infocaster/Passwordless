(function () {

    Controller.$inject = ['$scope', '$window', '$http', '$location', '$routeParams', 'passwordlessService'];
    function Controller($scope, $window, $http, $location, $routeParams, passwordlessService) {

        let $this = this;

        $this.submit = submit;
        $this.getButtonState = getButtonState;
        $this.getButtonDisabled = getButtonDisabled;

        let buttonDisabled = false;
        let buttonState = 'init';

        async function submit() {

            buttonState = 'busy';

            let p = new Passwordless.Client({
                apiKey: await passwordlessService.getPublicKey()
            });

            try {

                let token = await p.signin({});
                let redirect = `/umbraco-pwl-login?token=${encodeURIComponent(token)}`;
                if ($routeParams.returnPath) {
                    redirect += `&returnUrl=${$routeParams.returnPath}`;
                }

                $window.location.href = redirect;
                buttonState = 'success'

            } catch (e) {

                buttonState = 'error'
            }
        }

        function getButtonState() {
            return buttonState;
        }

        function getButtonDisabled() {
            return buttonDisabled;
        }

        function getUsername() {
            return $scope.$parent.$parent.vm.login;
        }
    }

    Directive.$inject = [];
    function Directive() {

        let directive = {
            transclude: false,
            restrict: 'E',
            replace: true,
            templateUrl: '/app_plugins/Passwordless/login.html',
            controller: 'passwordless.login',
            controllerAs: 'vm'
        };

        return directive;
    }

    angular.module('umbraco').directive('pwlLogin', Directive);
    angular.module('umbraco').controller('passwordless.login', Controller);

})();