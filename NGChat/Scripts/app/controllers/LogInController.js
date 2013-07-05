'use strict';

angular
    .module('chat.controllers')
    .controller('LogInController', ['$scope', '$location', 'userFactory', function ($scope, $location, userFactory) {
        var LOGIN_BTN_IDLE_TEXT = 'Przejdź do czatu',
            LOGIN_BTN_PROGRESS_TEXT = 'Proszę czekać...';

        $scope.username = '';
        $scope.loginInProgress = false;
        $scope.loginBtnText = LOGIN_BTN_IDLE_TEXT;
        $scope.errors = [];

        $scope.$watch(
            'loginInProgress',
            function (newValue, oldValue) {
                if (newValue != oldValue) {
                    if (newValue)
                        $scope.loginBtnText = LOGIN_BTN_PROGRESS_TEXT;
                    else
                        $scope.loginBtnText = LOGIN_BTN_IDLE_TEXT;
                }
            }, true);

        $scope.login = function () {
            if (!$scope.loginInProgress) {
                $scope.loginInProgress = true;

                userFactory.login($scope.username)
                    .success(function (data, status, headers, config) {
                        $scope.loginInProgress = false;

                        if (data && data.success) {
                            $location.path('/chat');
                        } else
                            $scope.errors = data.errors;
                    })
                    .error(function (data, status, headers, config) {
                        $scope.loginInProgress = false;
                        $scope.errors = [{
                            key: 'username',
                            messages: ['Wystąpił błąd, proszę spróbować później.']
                        }];
                    });
            }
        };
    }]);
