app.directive('phoneNumberSelector', function (apiService) {
    return {
        restrict: 'E',
        scope: {
            selectedId: '=',
            clientId: '=?' 
        },
        template: '<select class="form-control" ng-model="selectedId" ng-options="p.ID as p.Number for p in phoneNumbers"><option value="">-- Select Phone Number --</option></select>',
        link: function (scope, element, attrs) {
            scope.phoneNumbers = [];

            scope.loadNumbers = function () {
                if (scope.clientId) {
                    apiService.getActiveClientPhoneNumbers(scope.clientId).then(function (response) {
                        scope.phoneNumbers = response.data;
                    });
                } else {
                    apiService.getPhoneNumbers().then(function (response) {
                        scope.phoneNumbers = response.data;
                    });
                }
            };

            scope.$watch('clientId', function (newValue, oldValue) {
                scope.loadNumbers();
            });

            scope.getData = function () {
                return scope.selectedId;
            };
        }
    };
});