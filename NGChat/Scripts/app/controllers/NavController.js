'use strict';

angular
    .module('chat.controllers')
    .controller('NavController', ['$scope', '$location', 'userFactory', 'enumFactory', 'chatFactory', function ($scope, $location, userFactory, enumFactory, chatFactory) {
        $scope.rightDropdown = {
            show: false,
            username: ''
        };

        $scope.$watch(
            function () {
                return userFactory.authState;
            },
            function (newValue, oldValue) {
                $scope.rightDropdown.show = (newValue === enumFactory.authState.authenticated);
            }, true);

        $scope.$watch(
            function () {
                return userFactory.user;
            },
            function (newValue, oldValue) {
                if (newValue != null)
                    $scope.rightDropdown.username = newValue.name;
                else
                    $scope.rightDropdown.username = '';
            }, true);

        $scope.logout = function () {
            chatFactory.disconnect();
            userFactory.logout()
                .success(function (data) {
                    if (data && data.success)
                        $location.path('/');
                });
        };
    }]);
