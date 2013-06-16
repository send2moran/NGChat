'use strict';

angular
    .module('chat.services')
    .factory('chatFactory', ['$rootScope', '$http', '$q', '$timeout', 'enumFactory', 'userFactory', function ($rootScope, $http, $q, $timeout, enumFactory, userFactory) {
        var factory = {
            connectedUsers: [],
            messages: [],
            connectionState: enumFactory.connectionState.none
        },

        // hub data for managing signalr
        hub = $.connection.chatHub;

        function findConnectedUserIndex(userId) {
            var index = -1;

            angular.forEach(factory.connectedUsers, function (value, key) {
                if (value.id == userId) {
                    index = key;
                    return false;
                }
            });

            return index;
        };

        function connectedUserExists(userId) {
            return findConnectedUserIndex(userId) != -1;
        };

        // signalr hub dynamic functions
        hub.client.appendMessage = function (user, message) {
            factory.messages.push(factory.initChatMessageObject(
                userFactory.initUserObject(user.Id, user.Name),
                message,
                new Date(),
                userFactory.user != null && user.Id == userFactory.user.id ? 'author' : ''));

            if (!$rootScope.$root.$$phase)
                $rootScope.$apply();
        };

        hub.client.userConnected = function (user) {
            var newUser = {
                id: user.Id,
                name: user.Name
            };

            if (!connectedUserExists(newUser.id)) {
                factory.connectedUsers.push({ id: user.Id, name: user.Name });

                factory.messages.push(factory.initChatMessageObject(
                    null,
                    'Użytkownik ' + user.Name + ' dołączył do czata.',
                    new Date(),
                    'global'));

                if (!$rootScope.$root.$$phase)
                    $rootScope.$apply();
            }
        };

        hub.client.userDisconnected = function (user) {
            var disconnectedUser = {
                id: user.Id,
                name: user.Name
            };
            
            var disconnectedUserIndex = findConnectedUserIndex(disconnectedUser.id);

            if (disconnectedUserIndex != -1) {
                factory.messages.push(factory.initChatMessageObject(
                    null,
                    'Użytkownik ' + user.Name + ' wyszedł z czata.',
                    new Date(),
                    'global'));

                factory.connectedUsers.splice(disconnectedUserIndex, 1);

                if (!$rootScope.$root.$$phase)
                    $rootScope.$apply();
            }
        };

        $.connection.hub.reconnecting(function () {
            console.log('signalr reconnecting');

            factory.connectionState = enumFactory.connectionState.reconnecting;

            if (!$rootScope.$root.$$phase)
                $rootScope.$apply();
        });

        $.connection.hub.reconnected(function () {
            console.log('signalr reconnected');

            factory.connectionState = enumFactory.connectionState.connected;

            if (!$rootScope.$root.$$phase)
                $rootScope.$apply();
        });

        $.connection.hub.disconnected(function () {
            console.log('signalr disconnected');

            if (factory.connectionState !== enumFactory.connectionState.none)
                factory.connectionState = enumFactory.connectionState.disconnected;

            if (!$rootScope.$root.$$phase)
                $rootScope.$apply();
        });

        // public
        factory.initChatMessageObject = function (user, message, date, cssClasses) {
            return {
                user: user,
                message: message || '',
                date: date || new Date(),
                cssClasses: cssClasses || ''
            };
        };

        factory.connect = function () {
            var deferred = $q.defer();

            factory.connectionState = enumFactory.connectionState.connecting;

            $.connection.hub.start()
                .done(function () {
                    factory.connectionState = enumFactory.connectionState.connected;
                    factory.getConnectedUsers();

                    deferred.resolve();
                })
                .fail(function () {
                    factory.connectionState = enumFactory.connectionState.disconnected;

                    deferred.reject();
                });

            return deferred.promise;
        };

        factory.disconnect = function () {
            // it is important to first set the connection as none (default state after logging in)
            // so as not to notify "notifyChatConnection" directive
            factory.connectionState = enumFactory.connectionState.none;
            $.connection.hub.stop();
            factory.connectedUsers = [];
            factory.messages = [];
        };

        factory.sendMessage = function (message) {
            if (factory.isConnected())
                hub.server.sendMessage(message);
        };

        factory.getConnectedUsers = function () {
            return $http.get('/user/checkconnectedusers')
                .success(function (data, status, headers, config) {
                    if (data && data.success) {
                        factory.connectedUsers = angular.isArray(data.model) ? data.model : [];

                        if (!$rootScope.$root.$$phase)
                            $rootScope.$apply();
                    }
                });
        };

        factory.isConnected = function () {
            return factory.connectionState === enumFactory.connectionState.connected;
        };

        return factory;
    }]);
