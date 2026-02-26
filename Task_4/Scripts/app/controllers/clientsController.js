app.controller('clientsController', function ($scope, $uibModal, apiService) {
    $scope.clients = [];
    $scope.searchName = '';
    $scope.searchType = '';

    $scope.loadClients = function () {
        apiService.getClients().then(function (response) {
            $scope.clients = response.data;
        });
    };

    $scope.search = function () {
        // Same fail-safe for the clients search!
        if (!$scope.searchName && ($scope.searchType === null || $scope.searchType === '')) {
            $scope.loadClients();
        } else {
            apiService.searchClients($scope.searchName, $scope.searchType).then(function (response) {
                $scope.clients = response.data;
            });
        }
    };

    $scope.delete = function (id) {
        if (confirm('Are you sure you want to delete this client?')) {
            apiService.deleteClient(id).then(function () {
                $scope.loadClients();
            }, function (error) {
                if (error.status === 409) alert(error.data);
            });
        }
    };

    // --- 1. EDIT MODAL ---
    $scope.openModal = function (clientToEdit) {
        var modalInstance = $uibModal.open({
            templateUrl: '/Templates/clientModal.html',
            controller: 'clientModalController',
            resolve: {
                client: function () {
                    if (clientToEdit) {
                        var copy = angular.copy(clientToEdit);
                        if (copy.BirthDate) copy.BirthDate = new Date(copy.BirthDate);
                        return copy;
                    } else {
                        return { ID: 0, Name: '', Type: 0, BirthDate: null };
                    }
                }
            }
        });

        modalInstance.result.then(function (savedClient) {
            if (savedClient.Type === 1) savedClient.BirthDate = null;

            if (savedClient.ID === 0) {
                apiService.addClient(savedClient).then($scope.loadClients);
            } else {
                apiService.updateClient(savedClient).then($scope.loadClients);
            }
        });
    }; // <--- THIS is the bracket that likely swallowed your other functions!

    // --- 2. RESERVE MODAL ---
    $scope.openReserveModal = function (clientToReserve) {
        var modalInstance = $uibModal.open({
            templateUrl: '/Templates/reserveModal.html',
            controller: 'reserveModalController',
            resolve: { client: function () { return angular.copy(clientToReserve); } }
        });

        modalInstance.result.then(function (reservationData) {
            apiService.reservePhoneNumber(reservationData).then(function () {
                alert("Phone Number Reserved Successfully!");
            }, function (error) {
                console.error("Error reserving", error);
            });
        });
    };

    // --- 3. UNRESERVE MODAL ---
    $scope.openUnreserveModal = function (clientToUnreserve) {
        var modalInstance = $uibModal.open({
            templateUrl: '/Templates/unreserveModal.html',
            controller: 'unreserveModalController',
            resolve: { client: function () { return angular.copy(clientToUnreserve); } }
        });

        modalInstance.result.then(function (unreserveData) {
            apiService.unreservePhoneNumber(unreserveData).then(function () {
                alert("Phone Number Unreserved Successfully!");
            }, function (error) {
                console.error("Error unreserving", error);
            });
        });
    };
}); // <-- End of the main clientsController


// --- MODAL CONTROLLERS ---
app.controller('clientModalController', function ($scope, $uibModalInstance, client) {
    $scope.client = client;
    $scope.isEdit = client.ID !== 0;

    $scope.save = function () { $uibModalInstance.close($scope.client); };
    $scope.cancel = function () { $uibModalInstance.dismiss('cancel'); };
});

app.controller('reserveModalController', function ($scope, $uibModalInstance, client) {
    $scope.client = client;
    $scope.request = { ClientID: client.ID, PhoneNumberID: null };

    $scope.save = function () {
        if (!$scope.request.PhoneNumberID) { alert("Please select a number!"); return; }
        $uibModalInstance.close($scope.request);
    };
    $scope.cancel = function () { $uibModalInstance.dismiss('cancel'); };
});

app.controller('unreserveModalController', function ($scope, $uibModalInstance, client) {
    $scope.client = client;
    $scope.request = { ClientID: client.ID, PhoneNumberID: null };

    $scope.save = function () {
        if (!$scope.request.PhoneNumberID) { alert("Please select a number to unreserve!"); return; }
        $uibModalInstance.close($scope.request);
    };
    $scope.cancel = function () { $uibModalInstance.dismiss('cancel'); };
});