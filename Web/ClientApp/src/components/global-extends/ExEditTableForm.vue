<!--
 /**
  *
  * 表格行内表单编辑组件
  *
  * author: lanbo
  *
  * version: 0.1
  *
  * description: 表格行内表单编辑组件，由 el-form 及 el-table 组件组成，因此可以设置 el-form 及 el-table 组件的 props 和 events
  *
  * props:
  *     * 接收 el-form 的 props
  *     table-props: Object | el-table 的 props
  *     table-events: Object | el-table 的 events
  *     table-class: String, Array, Object | el-table 的 class
  *     table-style: String, Array, Object | el-table 的 style
  *
  * methods:
  *     validateEditingRow(function callback(valid, invalidFields) {}) | 验证正在编辑行，回调第一个参数为 true 则验证成功，否则验证失败，当需要添加行时，需要调用此方法验证通过后再添加
  *     validateAllRow(function callback(valid, invalidFields) {}) | 验证所有的行，当有编辑行时先验证编辑行，回调第一个参数为 true 则验证成功，否则验证失败
  *     setEditingRow(row, force) | 设置 row 为编辑行，row 必须为 data 中的某一行，也可以设置为 null（不显示编辑行），force 是否强制换（默认 false），若 froce 为 true 则直接设置，否则若有编辑行则会先验证，通过后才设置
  *
  * changelog
  *     v0.1 2018-11-21 by lanbo | 基本功能
  *
  */
-->
<template>
    <el-form class="ex-edit-table-form" ref="exEditTableForm" v-bind="$attrs" :model="editingRow">
        <el-table
            ref="exEditTable"
            class="ui-table--edit"
            :class="tableClass"
            :style="tableStyle"
            v-bind="tablePropsComputed"
            v-on="tableEventsComputed"
        >
            <slot :editingRow="editingRow"></slot>
        </el-table>
    </el-form>
</template>

<script>
import AsyncValidator from 'async-validator';

export default {
    name: 'ExEditTableForm',
    inheritAttrs: false,
    props: {
        tableProps: {
            type: Object,
            default: () => ({})
        },
        tableEvents: {
            type: Object,
            default: () => ({})
        },
        tableClass: {
            type: [String, Array, Object],
            default: ''
        },
        tableStyle: {
            type: [String, Array, Object],
            default: ''
        },
        rowSave: {
            type: Function
        }
    },
    data() {
        return {
            editingRow: null
        };
    },
    computed: {
        tablePropsComputed() {
            return {
                stripe: true,
                ...this.tableProps
            };
        },
        tableEventsComputed() {
            return {
                ...this.tableEvents,
                'row-click': this.handleRowClick
            };
        },
        tableData() {
            return this.tableProps.data || [];
        },
        rules() {
            return this.$attrs.rules || {};
        }
    },
    methods: {
        // 提供给外部校验使用
        validateEditingRow(callback) {
            let cb = callback;
            let promise;
            if (!this._.isFunction(cb)) {
                promise = new window.Promise((resolve, reject) => {
                    cb = function(valid, invalidFields) {
                        valid ? resolve(valid) : reject(invalidFields);
                    };
                });
            }

            if (this.editingRow) {
                this.handleValidateEditingRow()
                    .then(() => {
                        cb(true);
                    })
                    .catch(invalidFields => {
                        cb(false, invalidFields);
                    });
            } else {
                cb(true);
            }

            return promise;
        },
        // 提供给外部进行全部校验
        validateAllRow(callback) {
            let cb = callback;
            let promise;

            if (!this._.isFunction(cb)) {
                promise = new window.Promise((resolve, reject) => {
                    cb = function(valid, invalidFields) {
                        valid ? resolve(valid) : reject(invalidFields);
                    };
                });
            }

            if (this.editingRow) {
                this.handleValidateEditingRow()
                    .then(() => {
                        // 验证通过，验证所有的row
                        this.handleValidateAllRow().then(({valid, invalidFields}) => {
                            cb(valid, invalidFields);
                        });
                    })
                    .catch(invalidFields => {
                        cb(false, invalidFields);
                    });
            } else {
                // 验证所有的row
                this.handleValidateAllRow().then(({valid, invalidFields}) => {
                    cb(valid, invalidFields);
                });
            }

            return promise;
        },
        // 提供给外部设置编辑行
        setEditingRow(row, force = false) {
            if (force) {
                this.setEditingRowAndUpdateSlot(row);
            } else {
                this.validateEditingRow(valid => {
                    if (valid) {
                        this.setEditingRowAndUpdateSlot(row);
                    }
                });
            }
        },
        async handleValidateAllRow() {
            for (let index = 0; index < this.tableData.length; index++) {
                const model = this.tableData[index];

                try {
                    await this.asyncValidator(model);
                } catch (invalidFields) {
                    this.setEditingRow(model);
                    setTimeout(() => {
                        this.handleValidateEditingRow();
                    }, 0);
                    return {valid: false, invalidFields};
                }
            }

            return {valid: true};
        },
        asyncValidator(model) {
            return new Promise((res, rej) => {
                const validator = new AsyncValidator(this.rules);
                validator.validate(model, {firstFields: true}, (errors, invalidFields) => {
                    if (errors) {
                        rej(invalidFields);
                    } else {
                        res();
                    }
                });
            });
        },
        async handleRowClick(...args) {
            const [row, column, event] = args;
            event.stopPropagation();
            if (column && column.type === 'action') {
                return;
            }
            this.emitEventRowClick(...args);

            // 如果是当前行，则不做任何修改
            if (this.editingRow === row) {
                return;
            }

            await this.changeEditingRow(row);
        },
        emitEventRowClick(...args) {
            if (this.tableEvents['row-click']) {
                this.tableEvents['row-click'](...args);
            }
        },
        async handleDocumentClick() {
            await this.changeEditingRow(null);
        },
        handleValidateEditingRow() {
            return new Promise((res, rej) => {
                this.$refs.exEditTableForm.validate((valid, invalidFields) => {
                    if (valid) {
                        res();
                        // 当前行验证通过
                        if (typeof this.rowSave === 'function') {
                            this.rowSave(this.editingRow);
                        }
                    } else {
                        // 验证不通过
                        rej(invalidFields);
                        return false;
                    }
                });
            });
        },
        async changeEditingRow(row) {
            if (this.editingRow) {
                try {
                    await this.handleValidateEditingRow();
                    this.setEditingRowAndUpdateSlot(row);
                } catch (error) {
                    return;
                }
            } else {
                this.setEditingRowAndUpdateSlot(row);
            }
        },
        setEditingRowAndUpdateSlot(row) {
            this.editingRow = row;
            this.updateSlot();
        },
        updateSlot() {
            const that = this.$refs.exEditTable;
            that.store.commit('setData', that.data);
            if (that.$ready) {
                that.$nextTick(() => {
                    that.doLayout();
                });
            }
        }
    },
    mounted() {
        document.addEventListener('click', this.handleDocumentClick);
    },
    beforeDestroy() {
        document.removeEventListener('click', this.handleDocumentClick);
    }
};
</script>
<style lang="less" scoped>
</style>

