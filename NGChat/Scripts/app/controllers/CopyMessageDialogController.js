'use strict';

angular
    .module('chat.controllers')
    .controller('CopyMessageDialogController', ['$scope', 'dialog', 'message', function ($scope, dialog, message) {
        $scope.message = message;

        $scope.close = function (result) {
            dialog.close(result);
        };
    }]);