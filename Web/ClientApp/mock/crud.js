const Mock = require('mockjs')
const successCode = 20000
const list = Mock.mock({
  'array|100': [{
    'id': /\d{5,10}/,
    'string': '@cname',
    'int': '@integer(1, 100)',
    'datetime': '@datetime',
    'select': 'yes',
    'multiSelect': ['yes', 'no']
  }]
}).array

module.exports = [
  {
    url: '/api/crudSample/getList',
    type: 'get',
    response: config => {
      var pageSize = config.query.pageSize
      var pageIndex = config.query.pageIndex
      var items = list.slice((pageIndex - 1) * pageSize, pageIndex * pageSize)
      return {
        code: successCode,
        data: {
          items: items,
          total: 100,
          pageIndex: pageIndex,
          pageSize: pageSize
        }
      }
    }
  },
  {
    url: '/api/crudSample/save',
    type: 'post',
    response: config => {
      return {
        code: successCode
      }
    }
  },
  {
    url: '/api/crudSample/delete',
    type: 'delete',
    response: config => {
      return {
        code: successCode
      }
    }
  }
]
