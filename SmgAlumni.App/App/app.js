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

        $stateProvider.state('menu',
        {
            templateUrl: '/App/templates/menu.html'
        });

        //$stateProvider.state('home', {
        //    url: '/',
        //    templateUrl: '/App/templates/home/home.html'
        //});

        $stateProvider.state('login', {
            url: '/account/login',
            templateUrl: '/App/templates/account/login.html',
            controller: 'loginController',
        });
        $stateProvider.state('register', {
            url: '/account/register',
            templateUrl: '/App/templates/account/register.html',
            controller: 'registerController',
        });
        $stateProvider.state('successfullregistration', {
            url: '/account/register/success',
            templateUrl: '/App/templates/account/registersuccess.html'
        });
        $stateProvider.state('forgotpassword', {
            url: '/account/forgotpassword',
            templateUrl: '/App/templates/account/forgotPassword.html',
            controller: 'forgotPasswordController'
        });
        $stateProvider.state('resetpassword', {
            url: '/resetpassword/{guid}/{email}',
            templateUrl: '/App/templates/account/resetPassword.html',
            controller: 'resetPasswordController'
        });
        $stateProvider.state('homeauth', {
            url: '/',
            templateUrl: '/App/templates/home/homeAuth.html',
            controller: 'homeAuthController',
            authenticate: true
        });
        $stateProvider.state('homeauth.search', {
            url: 'search',
            templateUrl: '/App/templates/user/search.html',
            controller: 'userSearchController',
            authenticate: true
        });
        $stateProvider.state('homeauth.news', {
            url: 'news',
            templateUrl: '/App/templates/user/news.html',
            controller: 'userNewsController',
            authenticate: true
        });
        $stateProvider.state('homeauth.causes', {
            url: 'causes',
            templateUrl: '/App/templates/user/causes.html',
            controller: 'userCausesController',
            authenticate: true
        });
        $stateProvider.state('homeauth.listings', {
            url: 'listings',
            templateUrl: '/App/templates/user/listings.html',
            controller: 'userListingsController',
            authenticate: true
        });
        $stateProvider.state('homeauth.forum', {
            url: 'forum',
            templateUrl: '/App/templates/user/forum.html',
            controller: 'userForumController',
            authenticate: true
        });
        $stateProvider.state('homeauth.forumthread', {
            url: 'forumthread/:id',
            templateUrl: '/App/templates/user/forumThread.html',
            controller: 'userForumThreadController',
            authenticate: true
        });
        $stateProvider.state('account', {
            url: '/account',
            templateUrl: '/App/templates/account/accountBase.html',
            authenticate: true
        });
        $stateProvider.state('account.manageaccount', {
            url: '/manageaccount',
            templateUrl: '/App/templates/account/manageAccount.html',
            controller: 'manageAccountController',
            authenticate: true
        });
        $stateProvider.state('account.changepassword', {
            url: '/changepassword',
            templateUrl: '/App/templates/account/changePassword.html',
            controller: 'changePasswordController',
            authenticate: true
        });
        
        //admin
        $stateProvider.state('admin', {
            url: '/admin',
            templateUrl: '/App/templates/admin/adminBase.html',
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
            templateUrl: '/App/templates/masteradmin/masterAdminBase.html',
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
    }]).run(['$rootScope', '$state', 'authHelper', function ($rootScope, $state, authHelper) {
        $rootScope.$on("$stateChangeStart", function (event, toState) {
            if ((toState.authenticate) && !sessionStorage.authenticationData) {
                $state.transitionTo('login', { returnUrl: toState.name });
                event.preventDefault();
            }
            if (toState.name.indexOf('admin') > -1) {
                var authData = authHelper.getAuth();
                if (authData.roles.indexOf('Admin') == -1) {
                    $state.transitionTo('homeauth');
                    event.preventDefault();
                }
            }
            if (toState.name.indexOf('masteradmin') > -1) {
                var authData = authHelper.getAuth();
                if (authData.roles.indexOf('MasterAdmin') == -1) {
                    $state.transitionTo('homeauth');
                    event.preventDefault();
                }
            }
        });
    }
    ]);
