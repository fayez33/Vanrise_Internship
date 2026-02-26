app.directive('clientSelector', function (apiService) {
    return {
        restrict: 'E',
        scope: {
            selectedId: '=' 
        },
        template: '<select class="form-control" ng-model="selectedId" ng-options="c.ID as c.Name for c in clients"><option value="">-- Select Client --</option></select>',
        link: function (scope, element, attrs) {
            scope.clients = [];

            apiService.getClients().then(function (response) {
                scope.clients = response.data;
            });

            scope.getData = function () {
                return scope.selectedId;
            };
        }
    };
});