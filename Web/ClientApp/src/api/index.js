// 此文件不要修改，如果要增加接口，请增加其它js，并在此引入 
// axios用法
// axios.request(config)
// axios.get(url[, config])
// axios.delete(url[, config])
// axios.head(url[, config])
// axios.options(url[, config])
// axios.post(url[, data[, config]])
// axios.put(url[, data[, config]])
// axios.patch(url[, data[, config]])
// config如下
/**
 * method:get/post/....
 * data:{}，发送到body里的
 * params:{}，发送到url时的
 */

// 如果接口的数据比较小，项目不复杂，所有的接口统一放在这里

import request from '@/utils/request'
export * from '@/api/basic'

// crudSample接口
// axios.request(config)
// axios.get(url[, config])
// axios.delete(url[, config])
// axios.head(url[, config])
// axios.options(url[, config])
// axios.post(url[, data[, config]])
// axios.put(url[, data[, config]])
// axios.patch(url[, data[, config]])

export const getList = params => request.get('/api/crudSample/getList', { params })
export const add = data => request.post('/api/crudSample/save', data)
export const edit = data => request.post('/api/crudSample/save', data)
export const remove = ids => request.delete('/api/crudSample/delete', { data: ids })

// keyvalue
export const getKeyValue = key => request.get('/api/keyValue/Get', { params: { key } })

// 配置管理
export const configQueryPage = params => request.get('/api/config/queryPage', { params })
export const configQueryList = params => request.get('/api/config/QueryList', { params })
export const configQueryListTree = params => request.get('/api/config/QueryListTree', { params })
export const configFind = data => request.post('/api/config/find', data)
export const configRemove = data => request.post('/api/config/remove', data)
export const configSave = data => request.post('/api/config/save', data)

// 其它权限相关
export const login = data => request.post('/api/permission/login', data)
export const logout = data => request.post('/api/permission/logout', data)
export const getUserInfo = params => request.get('/api/Permission/getUserInfo', { params })
export const getCurrentLoginUserInfo = params => request.get('/api/Permission/getCurrentLoginUserInfo', { params })
export const getAllUserInfo = params => request.get('/api/Permission/getAllUserInfo', { params })
export const saveUser = data => request.post('/api/permission/saveUser', data)
export const removeUser = data => request.post('/api/permission/removeUser', data)
export const getAllRole = params => request.get('/api/Permission/getAllRole', { params })
export const saveRole = data => request.post('/api/permission/saveRole', data)
export const removeRole = data => request.post('/api/permission/removeRole', data)
export const initResource = params => request.get('/api/Permission/initResource', { params })
export const getAllResourceTreeInfo = params => request.get('/api/Permission/getAllResourceTreeInfo', { params })
export const getAllResourceRoles = params => request.get('/api/Permission/getAllResourceRoles', { params })
export const getOwnedResourceRoles = params => request.get('/api/Permission/getOwnedResourceRoles', { params })
export const saveResource = data => request.post('/api/permission/saveResource', data)
export const removeResource = data => request.post('/api/permission/removeResource', data)
export const getUserRoles = params => request.get('/api/Permission/getUserRoles', { params })
export const getRoleResources = params => request.get('/api/Permission/getRoleResources', { params })
export const setUserRoles = data => request.post('/api/permission/setUserRoles', data)
export const setRoleResources = data => request.post('/api/permission/setRoleResources', data)
