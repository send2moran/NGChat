'use strict';

angular
    .module('chat.controllers')
    .controller('HomeController', ['$rootScope', '$scope', 'userFactory', 'chatFactory', 'enumFactory', function ($rootScope, $scope, userFactory, chatFactory, enumFactory) {
        $scope.chat = chatFactory;
        $scope.chat.newMessage = '';

        if (chatFactory.connectionState === enumFactory.connectionState.notConnected)
            chatFactory.connect();

        $scope.sendMessage = function () {
            if ($scope.chat.newMessage.length > 0) {
                chatFactory.sendMessage($scope.chat.newMessage);
                $scope.chat.newMessage = '';
            }
        }
    }]);
