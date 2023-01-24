(function () {

    Controller.$inject = ['$scope', '$http', '$window', '$location', 'passwordlessService'];
    function Controller($scope, $http, $window, $location, passwordlessService) {

        let $this = this;

        $this.submit = submit;

        let buttonDisabled = false;
        let buttonState = 'busy';

        async function submit() {

            let p = new Passwordless.Client({
                apiKey: await passwordlessService.getPublicKey()
            });

            let response = await $http.get('/umbraco/backoffice/api/passwordlessapi/register');
            await p.register(response.data);

            let token = await p.signin({});
            const externalLinkCallback = '/umbraco/externallinklogincallback';
            $window.location.href = `/umbraco-pwl-login?token=${encodeURIComponent(token)}&returnUrl=${encodeURIComponent(externalLinkCallback)}`;
        }

        function getButtonState() {
            return buttonState;
        }

        function getButtonDisabled() {
            return buttonDisabled;
        }
    }

    Directive.$inject = [];
    function Directive() {

        let directive = {
            transclude: false,
            restrict: 'E',
            replace: true,
            templateUrl: '/app_plugins/Passwordless/register.html',
            controller: 'passwordless.register',
            controllerAs: 'vm'
        };

        return directive;
    }

    angular.module('umbraco').directive('pwlRegister', Directive);
    angular.module('umbraco').controller('passwordless.register', Controller);

})();