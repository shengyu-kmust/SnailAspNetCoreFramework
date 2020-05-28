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
export const getKeyValue = params => request.get('/api/keyValue/Get', { params })

// 用户管理
export const userQueryPage = params => request.get('/api/user/queryPage', { params })
export const userFind = data => request.post('/api/user/find', data)
export const userRemove = data => request.post('/api/user/remove', data)
export const userSave = ids => request.delete('/api/user/save', { data: ids })

// 角色管理
export const roleQueryPage = params => request.get('/api/role/queryPage', { params })
export const roleFind = data => request.post('/api/role/find', data)
export const roleRemove = data => request.post('/api/role/remove', data)
export const roleSave = ids => request.delete('/api/role/save', { data: ids })

// 其它权限相关
export const getUserRoles = params => request.get('/api/Permission/GetUserRoles', { params })
export const getAllRole = params => request.get('/api/Permission/GetAllRole', { params })
export const setUserRoles = data => request.post('/api/Permission/SetUserRoles', data)
export const queryListTree = params => request.get('/api/Config/QueryListTree', { params })
