app.controller('loginController', function ($scope, $rootScope, apiService) {
    $scope.credentials = { Username: '', Password: '' };
    $scope.errorMessage = '';

    $scope.login = function () {
        $scope.errorMessage = ''; // Clear previous errors

        apiService.login($scope.credentials).then(function (response) {
            $rootScope.isLoggedIn = true;
            $rootScope.loggedInUser = response.data.Username;

            // Save the role globally and in session storage
            $rootScope.userRole = response.data.Role;
            sessionStorage.setItem('jwtToken', response.data.Token);
            sessionStorage.setItem('userRole', response.data.Role);

            $scope.navigate('/Templates/devices.html');
        // ...
        },
            function (error) {
            // If the API returns 401 Unauthorized, show an error
            if (error.status === 401) {
                $scope.errorMessage = 'Invalid username or password.';
            } else {
                $scope.errorMessage = 'An error occurred connecting to the server.';
            }
        });
    };
});