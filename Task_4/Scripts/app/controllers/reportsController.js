app.controller('reportsController', function ($scope, apiService) {

    // --- Task 1: Clients Per Type Report ---
    $scope.clientsReport = [];
    $scope.filterType = ''; // Bound to your client-type-selector directive

    $scope.loadClientsReport = function () {
        apiService.getClientsPerTypeReport($scope.filterType).then(function (response) {
            $scope.clientsReport = response.data;
        }, function (error) {
            console.error('Error loading clients report:', error);
        });
    };

    // --- Task 2: Phone Numbers Status Per Device Report ---
    $scope.phoneReport = [];
    $scope.filterDevice = ''; // Bound to your device-selector directive
    $scope.filterStatus = ''; // Bound to the local select dropdown (1 for Reserved, 0 for Unreserved)

    $scope.loadPhoneReport = function () {
        apiService.getPhoneStatusReport($scope.filterDevice, $scope.filterStatus).then(function (response) {
            $scope.phoneReport = response.data;
        }, function (error) {
            console.error('Error loading phone status report:', error);
        });
    };
});