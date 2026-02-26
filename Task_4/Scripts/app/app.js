var app = angular.module('internshipApp', ['ui.bootstrap']);

app.controller('mainController', function ($scope) {
    
    $scope.currentView = '/Templates/devices.html';

    $scope.navigate = function (viewPath) {
        $scope.currentView = viewPath;
    };
});