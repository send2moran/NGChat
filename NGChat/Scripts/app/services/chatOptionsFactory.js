'use strict';

angular
    .module('chat.services')
    .factory('chatOptionsFactory', ['$rootScope', '$http', '$q', '$timeout', 'enumFactory', function ($rootScope, $http, $q, $timeout, enumFactory, userFactory) {
        var factory = {
            smileysEnabled: false,
            messagesTracking: true,
            soundNotificationEnabled: true,
            desktopNotificationEnabled: true,
            shorteningTooLongMessages: true
        };
        
        return factory;
    }]);
