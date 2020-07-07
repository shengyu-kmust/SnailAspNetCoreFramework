
import request from '@/utils/request'

// Demo管理
export const DemoQueryPage = params => request.get('/api/Demo/queryPage', { params })
export const DemoFind = data => request.post('/api/Demo/find', data)
export const DemoRemove = data => request.post('/api/Demo/remove', data)
export const DemoSave = data => request.post('/api/Demo/save', data)

