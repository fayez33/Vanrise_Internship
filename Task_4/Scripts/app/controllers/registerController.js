// Notice we injected $timeout here
app.controller('registerController', function ($scope, $timeout, apiService) {
    $scope.newAccount = { Username: '', Password: '', ConfirmPassword: '' };
    $scope.errorMessage = '';
    $scope.successMessage = '';
    $scope.isRegistering = false; // Tracks if the API call is in progress

    $scope.register = function () {
        $scope.errorMessage = '';
        $scope.successMessage = '';

        if ($scope.newAccount.Password !== $scope.newAccount.ConfirmPassword) {
            $scope.errorMessage = "Passwords do not match!";
            return;
        }

        // Lock the button to prevent double-clicking
        $scope.isRegistering = true;

        var credentials = {
            Username: $scope.newAccount.Username,
            Password: $scope.newAccount.Password
        };

        apiService.register(credentials).then(function (response) {
            $scope.successMessage = "Account created successfully! Redirecting to login...";
            $scope.newAccount = { Username: '', Password: '', ConfirmPassword: '' };

            // The UX Polish: Wait 2 seconds so they can read the success message, then navigate
            $timeout(function () {
                $scope.navigate('/Templates/login.html');
            }, 2000);

        }, function (error) {
            // Unlock the button if it fails so they can try again
            $scope.isRegistering = false;
            $scope.errorMessage = error.data && error.data.Message ? error.data.Message : 'Registration failed.';
        });
    };
});