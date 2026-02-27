// 1. Initialize the AngularJS application and inject UI-Bootstrap
var app = angular.module('internshipApp', ['ui.bootstrap']);
// Intercept all HTTP requests and inject the JWT token
app.config(function ($httpProvider) {
    $httpProvider.interceptors.push(function ($q) {
        return {
            'request': function (config) {
                var token = sessionStorage.getItem('jwtToken');
                if (token) {
                    // Attach token using the standard Bearer format
                    config.headers.Authorization = 'Bearer ' + token;
                }
                return config;
            }
        };
    });
});

// 2. Set the global state when the app first runs
app.run(function ($rootScope) {
    // Start the app in a strictly logged-out state
    $rootScope.isLoggedIn = false;
    $rootScope.loggedInUser = '';
});

// 3. The main controller that wraps the whole Index.cshtml page
app.controller('mainController', function ($scope, $rootScope) {

    // Default the starting page to the login screen instead of devices
    $scope.currentView = '/Templates/login.html';

    // Function to change pages
    $scope.navigate = function (view) {
        $scope.currentView = view;
    };

    // Function to handle logging out
    // Function to handle logging out
    $scope.logout = function () {
        // Clear global variables
        $rootScope.isLoggedIn = false;
        $rootScope.loggedInUser = '';
        $rootScope.userRole = '';

        // Destroy both the token and the role from browser storage
        sessionStorage.removeItem('jwtToken');
        sessionStorage.removeItem('userRole');

        // Kick the user back to the login screen
        $scope.navigate('/Templates/login.html');
    };
});