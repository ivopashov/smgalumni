'use strict';

var app = angular.module('app', ['ngCkeditor', 'ngSanitize', 'ui.bootstrap', 'ui.router', 'angular-loading-bar', 'ngDialog', 'angularFileUpload']);



app.config([
    '$stateProvider', '$httpProvider', '$urlRouterProvider', 'ngDialogProvider',
    function ($stateProvider, $httpProvider, $urlRouterProvider, ngDialogProvider) {

        //================================================
        //add an request interceptor
        //================================================
        $httpProvider.interceptors.push('authInterceptor');

        //================================================
        // Make urls case insensitive
        //================================================
        $urlRouterProvider.rule(function ($injector, $location) {
            var path = $location.path(), normalized = path.toLowerCase();
            if (path != normalized) {
                $location.replace().path(normalized);
            }
        });

        //================================================
        // Dialog
        //================================================
        ngDialogProvider.setDefaults({
            className: 'ngdialog-theme-default',
            plain: false,
            showClose: true,
            closeByDocument: true,
            closeByEscape: true,
            appendTo: false,
        });

        //================================================
        // Routes
        //================================================ 
        $urlRouterProvider.otherwise("/");

        $stateProvider.state('menu', {
            url: '/',
            templateUrl: '/App/templates/home/menu.html',
            controller: 'menuController'
        });

        $stateProvider.state('menu.history', {
            url: 'history',
            templateUrl: '/App/templates/home/history.html',
        });

        $stateProvider.state('menu.login', {
            url: 'account/login/:returnUrl',
            templateUrl: '/App/templates/account/login.html',
            controller: 'loginController'
        });
        $stateProvider.state('menu.register', {
            url: 'account/register',
            templateUrl: '/App/templates/account/register.html',
            controller: 'registerController'
        });
        $stateProvider.state('menu.successfullregistration', {
            url: 'account/register/success',
            templateUrl: '/App/templates/account/registersuccess.html'
        });
        $stateProvider.state('menu.forgotpassword', {
            url: 'account/forgotpassword',
            templateUrl: '/App/templates/account/forgotPassword.html',
            controller: 'forgotPasswordController'
        });
        $stateProvider.state('menu.resetpassword', {
            url: 'resetpassword/{guid}/{email}',
            templateUrl: '/App/templates/account/resetPassword.html',
            controller: 'resetPasswordController'
        });
        $stateProvider.state('menu.search', {
            url: 'search',
            templateUrl: '/App/templates/user/search.html',
            controller: 'userSearchController',
            authenticate: true
        });
        $stateProvider.state('menu.news', {
            url: 'news',
            templateUrl: '/App/templates/user/news.html',
            controller: 'userNewsController'
        });
        $stateProvider.state('menu.causes', {
            url: 'causes',
            templateUrl: '/App/templates/user/causes.html',
            controller: 'userCausesController'
        });
        $stateProvider.state('menu.listings', {
            url: 'listings',
            templateUrl: '/App/templates/user/listings.html',
            controller: 'userListingsController'
        });
        $stateProvider.state('menu.forum', {
            url: 'forum',
            templateUrl: '/App/templates/user/forum.html',
            controller: 'userForumController'
        });
        $stateProvider.state('menu.forumthread', {
            url: 'forumthread/:id',
            templateUrl: '/App/templates/user/forumThread.html',
            controller: 'userForumThreadController'
        });
        $stateProvider.state('menu.account', {
            url: 'account',
            templateUrl: '/App/templates/account/accountBase.html',
            controller:'accountController',
            authenticate: true
        });
        //admin
        $stateProvider.state('admin', {
            url: '/admin',
            templateUrl: '/App/templates/admin/adminmenu.html',
            authenticate: true
        });
        $stateProvider.state('admin.verifyusers', {
            url: '/verifyusers',
            templateUrl: '/App/templates/admin/verifyUsers.html',
            controller: 'verifyUsersController',
            authenticate: true
        });
        $stateProvider.state('admin.news', {
            url: '/news',
            templateUrl: '/App/templates/admin/news.html',
            controller: 'adminNewsController',
            authenticate: true
        });
        $stateProvider.state('admin.causes', {
            url: '/causes',
            templateUrl: '/App/templates/admin/causes.html',
            controller: 'adminCausesController',
            authenticate: true
        });
        $stateProvider.state('admin.manageusers', {
            url: '/manage',
            templateUrl: '/App/templates/admin/manageUsers.html',
            controller: 'adminManageUsersController',
            authenticate: true
        });
        //masteradmin
        $stateProvider.state('masteradmin', {
            url: '/masteradmin',
            templateUrl: '/App/templates/masteradmin/masteradminmenu.html',
            authenticate: true
        });
        $stateProvider.state('masteradmin.manageroles', {
            url: '/manageroles',
            templateUrl: '/App/templates/masteradmin/manageroles.html',
            controller: 'manageRolesController',
            authenticate: true
        });
        $stateProvider.state('masteradmin.settings', {
            url: '/settings',
            templateUrl: '/App/templates/masteradmin/settings.html',
            controller: 'settingsController',
            authenticate: true
        });
    }])
    .run(['$rootScope', '$state', 'authHelper', function ($rootScope, $state, authHelper) {
        $rootScope.$on("$stateChangeStart", function (event, toState) {
            if ((toState.authenticate) && !sessionStorage.authenticationData) {
                $state.go('menu.login', { returnUrl: toState.name });
                event.preventDefault();
                return;
            }
            
            if (toState.name.indexOf('admin') > -1) {
                var authData = authHelper.getAuth();
                if (authData.roles.indexOf('Admin') == -1) {
                    $state.go('menu');
                    event.preventDefault();
                }
            }
            if (toState.name.indexOf('masteradmin') > -1) {
                var authData1 = authHelper.getAuth();
                if (authData1.roles.indexOf('MasterAdmin') == -1) {
                    $state.go('menu');
                    event.preventDefault();
                }
            }
        });
    }
    ]);
