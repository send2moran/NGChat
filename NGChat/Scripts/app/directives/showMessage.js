'use strict';

angular
    .module('chat.directives')
    .directive('showMessage', [function ($window, userFactory) {
        return {
            replace: false,
            restrict: 'EA',
            scope: {
                originalMessage: '&showMessage'
            },
            link: function (scope, element, attrs) {
                var $element = angular.element(element),
                    maxLines = 8,
                    lineHeight = parseInt($element.css('lineHeight'), 10),
                    maxHeight = maxLines * lineHeight,
                    originalMessage = scope.originalMessage(),
                    elementForTest = null,
                    testedElementHeight = 0;

                elementForTest = angular.element('<div></div>');
                elementForTest
                    .html(originalMessage)
                    .css('display', 'none')
                    .appendTo(element);

                testedElementHeight = elementForTest.height();
                elementForTest.remove();

                if (testedElementHeight > maxHeight) {
                    var BUTTON_SHOW_MORE_TXT = 'pokaż więcej',
                        BUTTON_SHOW_LESS_TXT = 'pokaż mniej',
                        shorterMessageElement =
                        '<div class="shorter" ' +
                             'style="height: ' + maxHeight + 'px" ' +
                             originalMessage + '</div>' +
                        '<button type="button" class="toggleMessage btn btn-link btn-mini">' +
                        BUTTON_SHOW_MORE_TXT + '</button>';

                    element[0].innerHTML = shorterMessageElement;

                    $element.find('.toggleMessage').bind('click', function () {
                        var $button = angular.element(this),
                            $message = $button.prev('.shorter'),
                            currentHeight = parseInt($message.height(), 10);

                        if (currentHeight == maxHeight) {
                            $message.css('height', 'auto');
                            $button.text(BUTTON_SHOW_LESS_TXT);
                        } else {
                            $message.css('height', maxHeight + 'px');
                            $button.text(BUTTON_SHOW_MORE_TXT);
                        }
                    });
                } else
                    element[0].innerHTML = originalMessage;
            }
        };
    }]);
