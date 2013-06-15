'use strict';

angular
    .module('chat.controllers')
    .controller('HomeController', ['$rootScope', '$scope', 'userFactory', 'chatFactory', function ($rootScope, $scope, userFactory, chatFactory) {
        $scope.chat = chatFactory;
        $scope.chat.newMessage = '';

        chatFactory.connect();

        $scope.sendMessage = function () {
            if ($scope.chat.newMessage.length > 0) {
                chatFactory.sendMessage($scope.chat.newMessage);
                $scope.chat.newMessage = '';
            }
        }
    }]);
