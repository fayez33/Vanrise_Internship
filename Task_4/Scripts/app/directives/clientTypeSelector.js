app.directive('clientTypeSelector', function () {
    return {
        restrict: 'E',
        scope: {
            selectedType: '=' 
        },
        template: '<select class="form-control" ng-model="selectedType" ng-options="t.value as t.name for t in types"><option value="">-- Select Client Type --</option></select>',
        link: function (scope, element, attrs) {
            scope.types = [
                { name: 'Individual', value: 0 },
                { name: 'Organization', value: 1 }
            ];

            scope.getData = function () {
                return scope.selectedType;
            };
        }
    };
});