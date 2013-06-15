'use strict';

angular.module('chat.services', []);
angular.module('chat.directives', []);
angular.module('chat.controllers', ['ui.bootstrap', 'chat.services', 'chat.directives', 'ngSanitize']);
angular.module('chat.filters', []);
angular.module('chat', ['chat.controllers']);
