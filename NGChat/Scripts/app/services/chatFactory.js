'use strict';

angular
    .module('chat.services')
    .factory('chatFactory', ['$rootScope', '$http', '$q', '$timeout', 'enumFactory', 'userFactory', function ($rootScope, $http, $q, $timeout, enumFactory, userFactory) {
        var factory = {
            connectedUsers: [],
            messages: [],
            connectionState: enumFactory.connectionState.notConnected
        },

        // hub data for managing signalr
        hub = $.connection.chatHub;

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

        hub.client.userConnected = function (username) {
            // user id missing
            factory.connectedUsers.push({ id: -1, name: username });

            factory.messages.push(factory.initChatMessageObject(
                null,
                'Użytkownik ' + username + ' dołączył do czata.',
                new Date(),
                'global'));

            if (!$rootScope.$root.$$phase)
                $rootScope.$apply();
        };

        hub.client.userDisconnected = function (username) {
            var index = -1;

            factory.messages.push(factory.initChatMessageObject(
                null,
                'Użytkownik ' + username + ' wyszedł z czata.',
                new Date(),
                'global'));

            angular.forEach(factory.connectedUsers, function (value, key) {
                if (value.name == username) {
                    index = key;
                    return false;
                }
            });

            if (index > 0) {
                factory.connectedUsers.splice(index, 1);
                
                if (!$rootScope.$root.$$phase)
                    $rootScope.$apply();
            }
        };

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

            factory.connectionState = enumFactory.connectionState.inProgress;

            $.connection.hub.start()
                .done(function () {
                    factory.connectionState = enumFactory.connectionState.connected;
                    factory.getConnectedUsers();

                    deferred.resolve();
                })
                .fail(function () {
                    factory.connectionState = enumFactory.connectionState.notConnected;

                    deferred.reject();
                });

            return deferred.promise;
        };

        factory.disconnect = function () {
            $.connection.hub.stop();
            factory.connectionState = enumFactory.connectionState.notConnected;
            factory.connectedUsers = [];
            factory.messages = [];
        };

        factory.sendMessage = function (message) {
            if (factory.isConnected())
                hub.server.sendMessage(message);
        };

        factory.getConnectedUsers = function () {
            console.log('getConnectedUsers');

            return $http.get('/user/checkconnectedusers')
                .success(function (data, status, headers, config) {
                    if (data && data.success) {
                        if (angular.isArray(data.model)) {
                            angular.forEach(data.model, function (value, key) {
                                this.push({ id: value.id, name: value.name });
                            }, factory.connectedUsers);

                            if (!$rootScope.$root.$$phase)
                                $rootScope.$apply();
                        }
                    }
                })
                .error(function (data, status, headers, config) {

                });
        };

        factory.isConnected = function () {
            return factory.connectionState === enumFactory.connectionState.connected;
        };

        return factory;
    }]);
