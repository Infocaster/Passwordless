(function () {

    Service.$inject = ['$http', 'umbRequestHelper'];
    function Service($http, umbRequestHelper) {

        return {

            getPublicKey: getPublicKey
        };

        function getPublicKey() {
            return umbRequestHelper.resourcePromise($http.get('/umbraco/backoffice/api/passwordlessapi/publickey'), 'Failed to get public key from server');
        }
    }

    angular.module('umbraco').service('passwordlessService', Service);

})();