app.controller('phoneNumbersController', function ($scope, $uibModal, apiService) {
    $scope.phoneNumbers = [];
    $scope.searchNumber = '';
    $scope.searchDevice = ''; // Bound to the device-selector directive

    $scope.loadPhoneNumbers = function () {
        apiService.getPhoneNumbers().then(function (response) {
            $scope.phoneNumbers = response.data;
        });
    };

    $scope.search = function () {
        apiService.searchPhoneNumbers($scope.searchNumber, $scope.searchDevice).then(function (response) {
            $scope.phoneNumbers = response.data;
        });
    };

    $scope.delete = function (id) {
        if (confirm('Are you sure you want to delete this phone number?')) {
            apiService.deletePhoneNumber(id).then(function () {
                $scope.loadPhoneNumbers();
            }, function (error) {
                if (error.status === 409) alert(error.data);
            });
        }
    };

    $scope.openModal = function (phoneToEdit) {
        var modalInstance = $uibModal.open({
            templateUrl: '/Templates/phoneNumberModal.html',
            controller: 'phoneNumberModalController',
            resolve: {
                phone: function () {
                    // Note: Your C# model expects ID, Number, DeviceID
                    return phoneToEdit ? angular.copy(phoneToEdit) : { ID: 0, Number: '', DeviceID: null };
                }
            }
        });

        modalInstance.result.then(function (savedPhone) {
            if (savedPhone.ID === 0) {
                apiService.addPhoneNumber(savedPhone).then($scope.loadPhoneNumbers);
            } else {
                apiService.updatePhoneNumber(savedPhone).then($scope.loadPhoneNumbers);
            }
        });
    };
});

app.controller('phoneNumberModalController', function ($scope, $uibModalInstance, phone) {
    $scope.phone = phone;
    $scope.isEdit = phone.ID !== 0;

    $scope.save = function () {
        // Simple validation to ensure a device is selected
        if (!$scope.phone.DeviceID) {
            alert('Please select a device.');
            return;
        }
        $uibModalInstance.close($scope.phone);
    };
    $scope.cancel = function () { $uibModalInstance.dismiss('cancel'); };
});