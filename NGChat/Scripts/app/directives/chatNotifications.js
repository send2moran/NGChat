'use strict';

angular
    .module('chat.directives')
    .directive('chatNotifications', ['$window', '$document', function ($window, $document) {
        return {
            replace: false,
            restrict: 'EA',
            template: '<audio id="newMessageSound">' +
                          '<source src="/Content/sounds/new-message.ogg" type="audio/ogg">' +
                          '<source src="/Content/sounds/new-message.mp3" type="audio/mp3">' +
                          '<source src="/Content/sounds/new-message.wav" type="audio/wave">' +
                      '</audio>' +
                      '<audio id="connectDisconnectSound">' +
                          '<source src="/Content/sounds/connect-disconnect.ogg" type="audio/ogg">' +
                          '<source src="/Content/sounds/connect-disconnect.mp3" type="audio/mp3">' +
                          '<source src="/Content/sounds/connect-disconnect.wav" type="audio/wave">' +
                      '</audio>',
            link: function (scope, element, attrs) {
                function isWindowActive() {
                    return $window.document.hasFocus();
                };

                function canShowNotify() {
                    return notify.isSupported && notify.permissionLevel() == notify.PERMISSION_GRANTED;
                }

                scope.$watch(attrs.chatNotifications, function (newValue, oldValue) {
                    var lastMessage = newValue[newValue.length - 1];

                    if (!isWindowActive() && lastMessage && lastMessage.cssClasses != 'global') {
                        element.find('#newMessageSound')[0].play();

                        if (canShowNotify()) {
                            var message = lastMessage.message;

                            if (message.length > 30)
                                message = message.substring(0, 30) + ' ...';

                            var messageNotify = notify.createNotification(
                                "Nowa wiadomość od \"" + lastMessage.user.name + "\"",
                                {
                                    body: message,
                                    icon: "/Content/images/newMessage.ico",
                                    onClick: function () {
                                        $window.focus();
                                        this.cancel();
                                    }
                                });
                        }
                    }
                }, true);

                scope.$watch(attrs.connectedUsers, function (newValue, oldValue) {
                    if (!isWindowActive())
                        element.find('#connectDisconnectSound')[0].play();
                }, true);

                if (notify.isSupported) {
                    notify.config({ autoClose: 5000 });

                    angular.element('body').on('click', function () {
                        if (notify.permissionLevel() == notify.PERMISSION_DEFAULT)
                            notify.requestPermission();
                    });
                }
                
            }
        };
    }]);
