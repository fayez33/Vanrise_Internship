app.directive('deviceSelector', function (apiService) {
    return {
        restrict: 'E',
        scope: {
            selectedId: '='
        },
        template: '<select class="form-control" ng-model="selectedId" ng-options="d.ID as d.Name for d in devices"><option value="">-- Select Device --</option></select>',
        link: function (scope, element, attrs) {
            scope.devices = [];

            apiService.getDevices().then(function (response) {
                scope.devices = response.data;
            });

            scope.getData = function () {
                return scope.selectedId;
            };
        }
    };
});