'use strict';

angular
    .module('chat.controllers')
    .controller('LogInController', ['$scope', '$location', 'userFactory', function ($scope, $location, userFactory) {
        $scope.username = '';

        $scope.chooseNick = function () {
            userFactory.login($scope.username)
                .success(function (data, status, headers, config) {
                    if (data && data.success)
                        $location.path('/chat');
                })
                .error(function (data, status, headers, config) {

                });
        };
    }]);
