'use strict';

angular
    .module('chat.directives')
    .directive('submitTextarea', ['$rootScope', function ($rootScope) {
        return {
            replace: false,
            restrict: 'EA',
            scope: false,
            link: function (scope, element, attrs) {
                element.bind("keyup", function (e) {
                    if (e.keyCode == 13) {
                        if (e.ctrlKey) {
                            var val = this.value;
                            if (typeof this.selectionStart == "number" && typeof this.selectionEnd == "number") {
                                var start = this.selectionStart;
                                this.value = val.slice(0, start) + "\n" + val.slice(this.selectionEnd);
                                this.selectionStart = this.selectionEnd = start + 1;
                            } else if (document.selection && document.selection.createRange) {
                                this.focus();
                                var range = document.selection.createRange();
                                range.text = "\r\n";
                                range.collapse(false);
                                range.select();
                            }
                        } else
                            scope.$apply(attrs.submitTextarea);
                    }
                });

            }
        };
    }]);