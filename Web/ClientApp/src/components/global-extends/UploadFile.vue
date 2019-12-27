<template>
    <div>
        <div id="tableUp" v-if="uploadControl">
            <el-upload
                class="upload-demo"
                ref="upload"
                action="/api/v1/OrPMS/Accessory/UploadFile"
                :headers="headers"
                :on-success="handleSuccess"
                :data="uploadData"
                :auto-upload="false"
                :show-file-list="showFileList"
            >
                <el-button
                    slot="trigger"
                    style="display: inline-block;"
                    type="primary"
                    size="mini"
                >{{this.$t('file-upload.button.select-file')}}</el-button>
                <el-button
                    style="display: inline-block;margin-left: 10px;"
                    type="primary"
                    size="mini"
                    @click="submitUpload"
                >{{this.$t('file-upload.button.upload')}}</el-button>
                <el-button
                    type="primary"
                    size="mini"
                    style="display: inline-block;margin-left: 10px;"
                    ref="deleteFile"
                    @click="deleteFile"
                >{{this.$t('file-upload.button.delete-file')}}</el-button>
                <div
                    @click="uploadResult"
                    ref="uploadResult"
                    style=" display: inline-block;margin-left:15px;color:#F00"
                >{{this.$t('file-upload.button.show-result')}}</div>
            </el-upload>
        </div>
        <div id="tableDown" ref="table">
            <template>
                <el-table
                    ref="fileListTable"
                    :data="filesData"
                    tooltip-effect="dark"
                    @selection-change="selectHandle"
                          width="100%"
                >
                    <el-table-column type="selection" ></el-table-column>
                    <el-table-column
                        prop="name"
                        :label="this.$t('file-upload.table.file-dscription')"
                                     width="250px"
                    ></el-table-column>
                    <el-table-column
                        prop="accessorySize"
                        :label="this.$t('file-upload.table.file-size')"
                    ></el-table-column>
                    <el-table-column
                        prop="updateTime"
                        :label="this.$t('file-upload.table.upload-time')"
                                     width="150px"
                    ></el-table-column>
                    <el-table-column>
                        <template slot-scope="scope">
                            <el-button
                                @click="downloadFile(scope.row.id)"
                                type="primary"
                                icon="el-icon-download"
                            ></el-button>
                        </template>
                    </el-table-column>
                </el-table>
            </template>
        </div>
    </div>
</template>

<script>
import {fileDownload} from '@/utils';
import axiosInstance from '@/utils/axios.js';

export default {
    name: 'UploadFile',
    props: {
        dataId: {
            type: String,
            default: ''
        },
        type: {
            type: String,
            default: 'add'
        }
    },
    data() {
        return {
            showFileList: true,
            uploadControl: true,
            selectedRows: [],
            uploadData: {
                dataId: this.dataId
            },
            filesData: [
                {
                    name: '',
                    id: '',
                    accessorySize: '',
                    updateTime: ''
                }
            ]
        };
    },
    computed: {
        headers() {
            return {Authorization: `Bearer ${this.$store.state.token}`};
        }
    },
    created() {
        const {type} = this;
        if (type === 'edit') {
            this.uploadControl = true;
            this.getFiles();
        } else if (type === 'detail') {
            this.uploadControl = false;
            this.getFiles();
        }
        //this.getFiles();
    },
    methods: {
        selectHandle(val) {
            this.selectedRows = val;
        },
        submitUpload() {
            this.$refs.upload.submit();
        },
        handleSuccess() {
            this.getFiles();
        },
        //显示上传结果
        uploadResult() {
            if (!this.showFileList) {
                this.showFileList = true;
                this.$refs.uploadResult.innerText = this.$t('file-upload.button.hidden-result');
            } else {
                this.showFileList = false;
                this.$refs.uploadResult.innerText = this.$t('file-upload.button.show-result');
            }
        },
        //获取文件
        getFiles() {
            try {
                this.$client.getFileInfos({dataId: this.dataId}).then(data => {
                    // var arr = [];
                    // for (const i in data) {
                    //     var obj = {};
                    //     obj.fileId = data[i].id;
                    //     obj.fileDscription = data[i].name;
                    //     obj.fileSize = data[i].accessorySize;
                    //     obj.uploadTime = data[i].creatTime;
                    //     arr.push(obj);
                    // }
                    this.filesData = data;
                    if (data.length > 0) {
                        for (const i in data) {
                            if (!data[i].accessorySize) {
                                this.filesData[i].accessorySize = '';
                            }
                            var num = 1024.0; //byte
                            if (data[i].accessorySize < num) {
                                this.filesData[i].accessorySize = this.filesData[i].accessorySize + 'B';
                            } else if (data[i].accessorySize < Math.pow(num, 2)) {
                                this.filesData[i].accessorySize = (data[i].accessorySize / num).toFixed(2) + 'K'; //kb
                            } else if (data[i].accessorySize < Math.pow(num, 3)) {
                                this.filesData[i].accessorySize =
                                    (data[i].accessorySize / Math.pow(num, 2)).toFixed(2) + 'M'; //M
                            } else if (data[i].accessorySize < Math.pow(num, 4)) {
                                this.filesData[i].accessorySize =
                                    (data[i].accessorySize / Math.pow(num, 3)).toFixed(2) + 'G'; //G
                            } else if (data[i].accessorySize >= Math.pow(num, 4)) {
                                this.filesData[i].accessorySize =
                                    (data[i].accessorySize / Math.pow(num, 4)).toFixed(2) + 'T'; //T
                            }
                        }
                    }
                });
            } catch (error) {
                this.$msgbox({
                    type: 'error',
                    message: error.message
                });
            }
        },

        //删除选中文件
        deleteFile() {
            const {selectedRows} = this;
            const ids = selectedRows.map(item => item.id);
            if (ids && ids.length > 0) {
                this.$confirm(this.$t('common.confirm.delete'), this.$t('common.warning'), {
                    confirmButtonClass: 'el-button el-button--mini el-button--primary',
                    type: 'warning'
                })
                    .then(() => {
                        this.$client
                            .deleteFile(ids)
                            .then(() => {
                                this.$refs.fileListTable.clearSelection();
                                this.getFiles();
                            })
                            .catch(err => {
                                this.$message.error(err.msg);
                                this.getFiles();
                            });
                    })
                    .catch(() => {});
            } else {
                this.$msgbox({
                    type: 'error',
                    message: this.$t('file-upload.tip.delete-tip')
                });
                return;
            }
        },
        //文件下载
        downloadFile(id) {
            fileDownload('v1/OrPMS/Accessory/GetFile', {data: {id}})
                .then(data => {
                    console.log('success' + data);
                })
                .catch(e => {
                    this.$message({
                        message: this.$t('common.operate.failed'),
                        type: 'error'
                    });
                    console.log('error' + e.msg);
                });
        }
    }
};</script>


<style scoped lang="less">
</style>
