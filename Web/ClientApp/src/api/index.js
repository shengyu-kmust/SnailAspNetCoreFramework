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

