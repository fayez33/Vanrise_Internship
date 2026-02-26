app.controller('devicesController', function ($scope, $uibModal, apiService) {
    $scope.devices = [];
    $scope.searchText = '';

    // 1. Expose the load function to the scope
    $scope.loadDevices = function () {
        apiService.getDevices().then(function (response) {
            $scope.devices = response.data;
        }, function (error) {
            console.error('Error loading devices', error);
        });
    };

    // 2. Search function
    $scope.search = function () {
        var query = $scope.searchText;

        // If the box is empty, undefined, or just spaces, fetch ALL devices
        if (query === undefined || query === null || query === '') {
            apiService.getDevices().then(function (response) {
                $scope.devices = response.data;
            });
        }
        // Otherwise, run the search API
        else {
            apiService.searchDevices(query).then(function (response) {
                $scope.devices = response.data;
            });
        }
    };

    // 3. Modal function
    $scope.openModal = function (deviceToEdit) {
        var modalInstance = $uibModal.open({
            templateUrl: '/Templates/deviceModal.html',
            controller: 'deviceModalController',
            resolve: {
                device: function () {
                    return deviceToEdit ? angular.copy(deviceToEdit) : { ID: 0, Name: '' };
                }
            }
        });

        modalInstance.result.then(function (savedDevice) {
            if (savedDevice.ID === 0) {
                apiService.addDevice(savedDevice).then($scope.loadDevices);
            } else {
                apiService.updateDevice(savedDevice).then($scope.loadDevices);
            }
        });
    };

    // 4. Delete function (Fixed the load call!)
    $scope.delete = function (id) {
        if (confirm('Are you sure you want to delete this device?')) {
            apiService.deleteDevice(id).then(function (response) {
                $scope.loadDevices(); // <-- This was the bug! Fixed.
            }, function (error) {
                if (error.status === 409) { alert(error.data); }
                else { console.error('Error deleting device', error); }
            });
        }
    };

    // 5. Load data immediately when the controller initializes!
    $scope.loadDevices();
}); // <-- End of the first controller

// ---------------------------------------------------------
// THIS MUST BE OUTSIDE THE DEVICES CONTROLLER!
// ---------------------------------------------------------

app.controller('deviceModalController', function ($scope, $uibModalInstance, device) {
    $scope.device = device;
    $scope.isEdit = device.ID !== 0;

    $scope.save = function () {
        $uibModalInstance.close($scope.device);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});