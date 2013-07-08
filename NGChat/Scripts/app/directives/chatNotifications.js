'use strict';

angular
    .module('chat.directives')
    .directive('chatNotifications', ['$window', 'userFactory', function ($window, userFactory) {
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
            scope: {
                messages: '=chatNotifications',
                connectedUsers: '=connectedUsers',
                soundEnabled: '=notifySoundEnabled',
                desktopEnabled: '=notifyDesktopEnabled'
            },
            link: function (scope, element, attrs) {
                function isWindowActive() {
                    return $window.document.hasFocus();
                };

                function canShowNotify() {
                    return notify.isSupported && notify.permissionLevel() == notify.PERMISSION_GRANTED &&
                        (scope.desktopEnabled === undefined || scope.desktopEnabled === true);
                }

                function canPlaySound() {
                    return scope.soundEnabled === undefined || scope.soundEnabled === true;
                }

                scope.$watch('messages', function (newValue, oldValue) {
                    var lastMessage = newValue[newValue.length - 1];

                    if (!isWindowActive() &&
                        lastMessage &&
                        lastMessage.cssClasses != 'global' &&
                        lastMessage.user.id != userFactory.user.id) {

                        if (canPlaySound())
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

                scope.$watch('connectedUsers', function (newValue, oldValue) {
                    if (!isWindowActive() && canPlaySound())
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
