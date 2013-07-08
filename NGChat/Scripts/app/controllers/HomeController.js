'use strict';

angular
    .module('chat.controllers')
    .controller('HomeController', ['$rootScope', '$scope', '$timeout', 'userFactory', 'chatFactory', 'enumFactory', function ($rootScope, $scope, $timeout, userFactory, chatFactory, enumFactory) {
        var reconnectionDefaultTimeout = 3750,
            reconnectionMaxTimeout = 60000,
            reconnectionTimeout = reconnectionDefaultTimeout;

        $scope.chat = chatFactory;
        $scope.chat.newMessage = '';

        function reconnect(time) {
            $timeout(function () {
                if (chatFactory.connectionState === enumFactory.connectionState.disconnected) {
                    chatFactory.connect().then(function () {
                        reconnectionTimeout = reconnectionDefaultTimeout;
                    }, function () {
                        if (reconnectionTimeout < reconnectionMaxTimeout)
                            reconnectionTimeout = reconnectionTimeout * 2;

                        reconnect(reconnectionTimeout);
                    });
                }
            }, time);
        };

        if (chatFactory.connectionState === enumFactory.connectionState.none)
            chatFactory.connect();

        $scope.$watch(
            'chat.connectionState',
            function (newValue, oldValue) {
                if (newValue != oldValue && newValue == enumFactory.connectionState.disconnected)
                    reconnect(reconnectionTimeout);
            }, true);

        $scope.sendMessage = function () {
            if ($scope.chat.newMessage.length > 0 && chatFactory.isConnected()) {
                chatFactory.sendMessage($scope.chat.newMessage);
                $scope.chat.newMessage = '';
            }
        }

        $scope.clearMessages = function () {
            $scope.chat.messages = [];
        };
    }]);
