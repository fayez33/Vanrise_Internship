app.controller('phoneNumberReservationsController', function ($scope, apiService) {
    $scope.reservations = [];
    $scope.searchClient = null; // Bound to client-selector directive
    $scope.searchPhone = null;  // Bound to phone-number-selector directive

    $scope.loadReservations = function () {
        apiService.getReservations().then(function (response) {
            $scope.reservations = response.data;
        });
    };

    $scope.search = function () {
        apiService.searchReservations($scope.searchClient, $scope.searchPhone).then(function (response) {
            $scope.reservations = response.data;
        });
    };
});