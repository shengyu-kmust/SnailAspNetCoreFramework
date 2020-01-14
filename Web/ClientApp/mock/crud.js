import Mock from 'mockjs'
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
export default [
  {
    url: '/api/crudSample/getList',
    type: 'get',
    response: config => {
      console.log('-------')
      var pageSize = config.query.pageSize
      var currentPage = config.query.currentPage
      var items = list.slice((currentPage - 1) * pageSize, currentPage * pageSize)
      return {
        code: successCode,
        data: {
          items: items,
          total: 100,
          currentPage: currentPage,
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
